using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWizard;
    [SerializeField]
    private GameObject _rightWizard;
    [SerializeField]
    private GameObject _cardHolder;
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private List<RenderTexture> RenderTextures = new List<RenderTexture>();
    [SerializeField]
    private GameObject _eventSystem;

    private GameManager _gameManager;

    private bool _isSecondPlayer = false;

    private int _currentCardCost = 0;
    private int _currentGold = 0;

    List<GameObject> _players = new List<GameObject>();

    private void OnEnable()
    {
        transform.parent.GetComponent<TopDownMovement>().enabled = false;

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _players = _gameManager.GetCurrentPlayers();

        if(_players.Count > 1)
        {
            if (_players[1] != transform.parent.gameObject)
            {
                _leftWizard.SetActive(false);
                _rightWizard.SetActive(true);
                _isSecondPlayer = true;
                return;
            }
        }

        _leftWizard.SetActive(true);
        _rightWizard.SetActive(false);
    }

    public void BuyCard()
    {
        if (_currentCardCost > _currentGold) return;
        _currentGold -= _currentCardCost;
        SpawnCard();
    }

    private void SpawnCard()
    {
        GameObject newCard = Instantiate(_cardPrefab, _cardHolder.transform);
        newCard.transform.parent = _cardHolder.transform;
        int index = 0;
        if (_isSecondPlayer) index = 1;

        Scratchers2D scratchScript = _cardHolder.GetComponentInChildren<Scratchers2D>();
        scratchScript.RT = RenderTextures[index];

        GameObject stageRenderer = GameObject.Find("StageRenderer");

        List<Camera> cameras = _players[index].GetComponentsInChildren<Camera>().ToList();
        foreach (Camera cam in cameras)
        {
            if (cam.name != "StageCamera") continue;
            cam.targetTexture = RenderTextures[index];
            scratchScript.StageCamera = cam;
        }

        scratchScript.StageRenderer = stageRenderer.GetComponent<MeshRenderer>();
        scratchScript.StageMeshFilter = stageRenderer.GetComponent<MeshFilter>();

        _eventSystem.SetActive(false);
    }
}
