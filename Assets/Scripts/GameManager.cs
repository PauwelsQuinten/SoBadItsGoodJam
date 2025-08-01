using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager _playerInputManager;

    [SerializeField]
    private Transform _player01Spawn;

    private int _playersJoined = 0;
    private List<GameObject> Players {  set;  get; }

    private void Awake()
    {
        Players = new List<GameObject>();
    }

    public void PlayerJoined(PlayerInput prefab)
    {
        GameObject player = prefab.gameObject;
        Players.Add(player);
        player.name = $"Player{Players.Count}";
        switch (_playersJoined)
        {
            case 0:
                player.transform.position = _player01Spawn.position;
                Debug.Log("Player 01 Joined");
                break;
            default:
                break;
        }
    }

    public List<GameObject> GetCurrentPlayers()
    {
        return Players;
    }
}
