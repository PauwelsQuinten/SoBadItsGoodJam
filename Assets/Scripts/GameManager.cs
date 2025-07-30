using NUnit.Framework;
using System.Collections.Generic;
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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayerJoined(PlayerInput prefab)
    {
        GameObject player = prefab.gameObject;
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
    }
}
