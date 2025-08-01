using UnityEngine;

public class SpawnGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameManager;
    void Start()
    {
        if(GameObject.FindAnyObjectByType<GameManager>() == null)
            Instantiate(_gameManager);
    }
}
