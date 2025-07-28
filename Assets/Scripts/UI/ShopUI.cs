using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWizard;
    [SerializeField]
    private GameObject _rightWizard;
    private GameManager _gameManager;

    private bool _isOnTheRight = false;

    private void OnEnable()
    {
        transform.parent.GetComponent<TopDownMovement>().enabled = false;

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        List<GameObject> players = _gameManager.GetCurrentPlayers();

        if(players.Count > 1)
        {
            if (players[1] != transform.parent.gameObject)
            {
                _leftWizard.SetActive(false);
                _rightWizard.SetActive(true);
                return;
            }
        }

        _leftWizard.SetActive(true);
        _rightWizard.SetActive(false);
    }
}
