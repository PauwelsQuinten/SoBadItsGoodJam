using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    private TextMeshPro _player1ScoreUI, _player2ScoreUI;

    private int _playersJoined = 0;
    private List<GameObject> Players {  set;  get; } = new List<GameObject>();

    private int _amountOfPlayersReady;

    private int _player1Score;
    private int _player2Score;

    private List<GameObject> _spawnPoints = new List<GameObject>();
    private List<GameObject> _HudUI = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();
         if (_spawnPoints[0].transform.parent.name == "Spawnpoint_Blue")
        {
            _playerSpawnPoints[0] = _spawnPoints[0].transform;
            _playerSpawnPoints[1] = _spawnPoints[1].transform;
        }
        else
        {
            _playerSpawnPoints[0] = _spawnPoints[1].transform;
            _playerSpawnPoints[1] = _spawnPoints[0].transform;
        }
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

    public void BackToLobbyPressed(Component sender, object obj)
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerReadiedUp(Component sender, object obj)
    {
        _amountOfPlayersReady++;
        if (_amountOfPlayersReady != Players.Count) return;

        StartCoroutine(DelayedGameStart(1));
    }

    IEnumerator DelayedGameStart(int delay)
    {
        yield return new WaitForSeconds(delay);

        _amountOfPlayersReady = 0;

        List<GameObject> canvases = GameObject.FindGameObjectsWithTag("Canvas").ToList();
        foreach (GameObject canv in canvases)
        {
            ShopUI shopUI = canv.GetComponent<ShopUI>();
            shopUI.EnableUI(false);
            shopUI.enabled = false;
        }

        foreach (GameObject player in Players)
        {
            player.GetComponent<PlayerHealth>().ResetHealth();
            player.GetComponent<SpellCasting>().ResetMana();
        }

        foreach (GameObject hud in _HudUI)
        {
            hud.SetActive(true);
        }
    }

    public void PlayerDied(Component sender, object obj)
    {
        if (_spawnPoints[0].transform.parent.name == "Spawnpoint_Blue")
        {
            Players[0].transform.position = _spawnPoints[0].transform.position + new Vector3(0, 1.5f, 0);
            Players[1].transform.position = _spawnPoints[1].transform.position + new Vector3(0, 1.5f, 0);
            Players[0].transform.rotation = _spawnPoints[0].transform.rotation;
            Players[1].transform.rotation = _spawnPoints[1].transform.rotation;
        }
        else
        {
            Players[0].transform.position = _spawnPoints[1].transform.position + new Vector3(0, 1.5f, 0);
            Players[1].transform.position = _spawnPoints[0].transform.position + new Vector3(0, 1.5f, 0);
            Players[0].transform.rotation = _spawnPoints[1].transform.rotation;
            Players[1].transform.rotation = _spawnPoints[0].transform.rotation;
        }

        foreach (GameObject hud in _HudUI)
        {
            hud.SetActive(false);
        }

        int index = 0;
        List<GameObject> canvases = GameObject.FindGameObjectsWithTag("Canvas").ToList();
        foreach (GameObject canv in canvases)
        {
            Canvas canvas = canv.GetComponent<Canvas>();
            ShopUI shopUI = canv.GetComponent<ShopUI>();

            canvas.enabled = true; 

            index++;

            GameObject player = canv.transform.parent.gameObject;

            if (player != obj as GameObject) continue;

            if (index == 1)
            {
                _player2Score++;
                Players[1].GetComponentInChildren<ShopUI>().GetCoins(5);
            }
            else
            {
                _player1Score++;
                Players[0].GetComponentInChildren<ShopUI>().GetCoins(5);
            }

        }

        index = 0;

        foreach (GameObject canv in canvases)
        {
            StartCoroutine(ShowScoreScreen(_player1Score, _player2Score, canv, index));

            index++;
        }

        CheckForWin();
    }

    private IEnumerator ShowScoreScreen(int player1Score, int player2Score, GameObject canv, int index)
    {
        ShopUI shopUI = canv.GetComponent<ShopUI>();
        List<GameObject> scoreBoards = GameObject.FindGameObjectsWithTag("ScoreBoard").ToList();

        ScoreBoardUI scoreBoardUI = scoreBoards[index].GetComponent<ScoreBoardUI>();
        scoreBoardUI.EnableUI(true);
        scoreBoardUI.SetScore(_player1Score, _player2Score);


        yield return new WaitForSeconds(5);

         scoreBoardUI.EnableUI(false);

        shopUI.enabled = true;
        shopUI.EnableUI(true);
        shopUI.Initialize();

    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScreen") InitializeStartScreen();
        if (scene.name == "Gameplay") initializeGameplayScene();
        if(scene.name == "Player1Won" || scene.name == "Player2Won") InitializeWinScene();
    }

    private void initializeGameplayScene()
    {
        _HudUI = GameObject.FindGameObjectsWithTag("HUD").ToList();
        foreach (GameObject hud in _HudUI)
        {
            hud.SetActive(false);
        }
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        if(_spawnPoints[0].transform.parent.name == "Spawnpoint_Blue")
        {
            Players[0].transform.position = _spawnPoints[0].transform.position + new Vector3(0, 1.5f, 0);
            Players[1].transform.position = _spawnPoints[1].transform.position + new Vector3(0, 1.5f, 0);
            Players[0].transform.rotation = _spawnPoints[0].transform.rotation;
            Players[1].transform.rotation = _spawnPoints[1].transform.rotation;
        }
        else
        {
            Players[0].transform.position = _spawnPoints[1].transform.position + new Vector3(0, 1.5f, 0);
            Players[1].transform.position = _spawnPoints[0].transform.position + new Vector3(0, 1.5f, 0);
            Players[0].transform.rotation = _spawnPoints[1].transform.rotation;
            Players[1].transform.rotation = _spawnPoints[0].transform.rotation;
        }
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
            shopUI.EnableUI(true);
            shopUI.Initialize();
        }
    }

    private void InitializeWinScene()
    {
        Destroy(Players[1]);
        Destroy(Players[0]);
        Players = new List<GameObject>();
        _playersJoined = 0;
        gameObject.GetComponent<PlayerInputManager>().enabled = false;
    }

    private void InitializeStartScreen()
    {
        gameObject.GetComponent<PlayerInputManager>().EnableJoining();
        StartCoroutine(EnableInputWithDelay());
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();
        if (_spawnPoints[0].transform.parent.name == "Spawnpoint_Blue")
        {
            _playerSpawnPoints[0] = _spawnPoints[0].transform;
            _playerSpawnPoints[1] = _spawnPoints[1].transform;
        }
        else
        {
            _playerSpawnPoints[0] = _spawnPoints[1].transform;
            _playerSpawnPoints[1] = _spawnPoints[0].transform;
        }
    }

    private void CheckForWin()
    {
        if (_player1Score >= 3) SceneManager.LoadScene(2);
        else if (_player2Score >= 3) SceneManager.LoadScene(3);
    }

    private IEnumerator EnableInputWithDelay()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<PlayerInputManager>().enabled = true;
    }
}
