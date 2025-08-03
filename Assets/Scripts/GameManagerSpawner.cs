using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameManager;
    void Start()
    {
        if(GameObject.FindAnyObjectByType<GameManager>() == null) Instantiate(_gameManager);
    }
}
