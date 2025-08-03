using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWizard;
    [SerializeField]
    private TextMeshPro _leftGoldText;
    [SerializeField]
    private GameObject _rightWizard;
    [SerializeField]
    private TextMeshPro _rightGoldText;
    [SerializeField]
    private GameObject _cardHolder;
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private List<RenderTexture> _renderTextures = new List<RenderTexture>();
    [SerializeField]
    private List<Material> _scratchMaterials = new List<Material>();
    [SerializeField]
    private GameObject _eventSystem;
    [SerializeField]
    private GameObject _SpellHolder;
    [SerializeField]
    private List<GameObject> _spells = new List<GameObject>();

    private GameManager _gameManager;

    private bool _isSecondPlayer = false;

    private int _currentCardCost = 5;
    private int _currentGold = 5;

    List<GameObject> _players = new List<GameObject>();

    private Spells _currentSpell = Spells.Default;
    private GameObject _spawnedSpell;

    private void OnEnable()
    {
        _gameManager = GameObject.FindAnyObjectByType<GameManager>();
        transform.parent.GetComponent<TopDownMovement>().enabled = false;


        _players = _gameManager.GetCurrentPlayers();

        if(_players.Count > 1)
        {
            if (_players[1] == transform.parent.gameObject)
            {
                _leftWizard.SetActive(false);
                _rightWizard.SetActive(true);

                _leftGoldText.transform.parent.gameObject.SetActive(false);
                _rightGoldText.transform.parent.gameObject.SetActive(true);

                _isSecondPlayer = true;

                if (_leftGoldText.transform.parent.gameObject.activeSelf) 
                    _leftGoldText.text = _currentGold.ToString();
                else _rightGoldText.text = _currentGold.ToString();

                return;
            }
        }

        _leftWizard.SetActive(true);
        _rightWizard.SetActive(false);

        _leftGoldText.transform.parent.gameObject.SetActive(true);
        _rightGoldText.transform.parent.gameObject.SetActive(false);

        if (_leftGoldText.transform.parent.gameObject.activeSelf) 
            _leftGoldText.text = _currentGold.ToString();
        else _rightGoldText.text = _currentGold.ToString();
    }


    public void BuyCard()
    {
        if (_currentCardCost > _currentGold) return; //Wat wil current gold zeggen? Van welke speler is deze gold?
        _currentGold -= _currentCardCost;
        SpawnCard();
    }

    public void GetCoins(int amount) 
    {
        _currentGold += amount;

        if (_leftGoldText.transform.parent.gameObject.activeSelf) _leftGoldText.text = _currentGold.ToString();
        else _rightGoldText.text = _currentGold.ToString();
    }

    public void ReadyUp()
    {
        _startingSprite = _readyButton.image.sprite;
        if (_isSecondPlayer)_readyButton.image.sprite = _readyUpSprite[1];
        else _readyButton.image.sprite = _readyUpSprite[0];
        SpriteState newSpriteState = _readyButton.spriteState;
        _startingSpriteState = newSpriteState;
        if (_isSecondPlayer) newSpriteState.selectedSprite = _readyUpSpriteHover[1];
        else newSpriteState.selectedSprite = _readyUpSpriteHover[0];
        _readyButton.spriteState = newSpriteState;
        Navigation newNavigation = _readyButton.navigation;
        _startNavigation = newNavigation;
        newNavigation.selectOnLeft = null;
        newNavigation.selectOnRight = null;
        _readyButton.navigation = newNavigation;
        _readyButton.enabled = false;
        _ReadiedUp.Raise(this, EventArgs.Empty);
    }

    public void ScratchingCompleted(Component sender, object obj)
    {
        if (sender.transform.parent.transform.parent.gameObject != transform.parent.gameObject) return;
        ChangeCurrentSpelUI((Spells)(obj as Spells?));
        _eventSystem.SetActive(true);
        StartCoroutine(DeleteCardWithDelay(sender.gameObject));
    }

    private void SpawnCard()
    {
        GameObject newCard = Instantiate(_cardPrefab, _cardHolder.transform);
        newCard.transform.parent = _cardHolder.transform;
        int index = 0;
        if (_isSecondPlayer) index = 1;

        Scratchers2D scratchScript = _cardHolder.GetComponentInChildren<Scratchers2D>();
        scratchScript.RT = _renderTextures[index];
        scratchScript.UnlitMaterial = _scratchMaterials[index];

        List<GameObject> stageRenderers = GameObject.FindGameObjectsWithTag("StageRenderer").ToList();

        foreach (GameObject stageRenderer in stageRenderers)
        {
            if (stageRenderer.transform.parent.transform.parent.gameObject != _players[index].gameObject) continue;
            scratchScript.StageRenderer = stageRenderer.GetComponent<MeshRenderer>();
            scratchScript.StageMeshFilter = stageRenderer.GetComponent<MeshFilter>();
        }

        List<Camera> cameras = _players[index].GetComponentsInChildren<Camera>().ToList();
        foreach (Camera cam in cameras)
        {
            if (cam.name != "StageCamera") continue;

            cam.targetTexture = _renderTextures[index];
            scratchScript.StageCamera = cam;
        }

        scratchScript.initialize();

        _eventSystem.SetActive(false);
    }

    private void ChangeCurrentSpelUI(Spells spell)
    {
        _currentSpell = spell;

        if (_spawnedSpell != null) Destroy(_spawnedSpell);

        switch (_currentSpell)
        {
            case Spells.Default:
                _spawnedSpell = Instantiate(_spells[0], _SpellHolder.transform);
                break;
            case Spells.WaterBall:
                _spawnedSpell = Instantiate(_spells[1], _SpellHolder.transform);
                break;
            case Spells.FireBall:
                _spawnedSpell = Instantiate(_spells[2], _SpellHolder.transform);
                break;
            case Spells.AcidBall:
                _spawnedSpell = Instantiate(_spells[3], _SpellHolder.transform);
                break;
        }
    }

    private IEnumerator DeleteCardWithDelay(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }

}
