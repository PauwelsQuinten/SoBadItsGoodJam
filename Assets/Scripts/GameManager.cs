using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager _playerInputManager;

    [SerializeField]
    private List<Transform> _playerSpawnPoints;
    [SerializeField]
    private List<Material> _playerMaterials;

    [SerializeField]
    private GameEvent _playerJoined;

    private int _playersJoined = 0;
    private List<GameObject> Players {  set;  get; } = new List<GameObject>();

    private int _amountOfPlayersReady;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayerJoined(PlayerInput prefab)
    {
        GameObject player = prefab.gameObject;
        DontDestroyOnLoad(player);
        Players.Add(player);
        player.name = $"Player{Players.Count}";
        _playerJoined.Raise(this, Players.Count - 1);
        switch (_playersJoined)
        {
            case 0:
                player.transform.position = _playerSpawnPoints[0].position;
                player.transform.rotation = _playerSpawnPoints[0].rotation;
                player.GetComponentInChildren<SkinnedMeshRenderer>().material = _playerMaterials[0];
                Debug.Log("Player 01 Joined");
                break;
            case 1:
                player.transform.position = _playerSpawnPoints[1].position;
                player.transform.rotation = _playerSpawnPoints[1].rotation;
                player.GetComponentInChildren<SkinnedMeshRenderer>().material = _playerMaterials[1];
                break;
            default:
                break;
        }
        _playersJoined++;
    }

    public List<GameObject> GetCurrentPlayers()
    {
        return Players;
    }

    public void PlayPressed(Component sender, object obj)
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void PlayerReadiedUp(Component sender, object obj)
    {
        _amountOfPlayersReady++;
        if (_amountOfPlayersReady != Players.Count) return;

        List<GameObject> canvases = GameObject.FindGameObjectsWithTag("Canvas").ToList();
        foreach (GameObject canv in canvases)
        {
            ShopUI shopUI = canv.GetComponent<ShopUI>();
            shopUI.EnableUI(false);
            shopUI.enabled = false;
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay") initializeGameplayScene();
    }

    private void initializeGameplayScene()
    {
        List<GameObject> cams = GameObject.FindGameObjectsWithTag("PlayerCam").ToList();
        foreach (GameObject cam in cams)
        {
            if (cam.name != "PlayerCamera") return;
            cam.GetComponent<Camera>().enabled = true;
        }
        List<GameObject> canvases = GameObject.FindGameObjectsWithTag("Canvas").ToList();
        foreach (GameObject canv in canvases)
        {
            Canvas canvas = canv.GetComponent<Canvas>();
            ShopUI shopUI = canv.GetComponent<ShopUI>();
            canvas.enabled = true;
            shopUI.enabled = true;
        }
    }
}
