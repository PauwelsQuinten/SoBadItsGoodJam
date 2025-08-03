using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellCasting : MonoBehaviour
{
    [Header("Every spell needs a rigidbody and a Collider!!!")]
    [SerializeField]
    private GameObject[] _spell;
    [SerializeField]
    private Sprite[] _spellIcons;
    [SerializeField]
    private Transform _castPoint;
    [SerializeField]
    private ManaBar _manaBar;
    [SerializeField]
    private float _spellSpeed = 5f, _maxMana = 20f;
    [SerializeField]
    public int _spellType = 0; //Later on change to a property 
    [SerializeField]
    private Image _spellIcon;

    [Header("Mana Regen Values")]
    [Tooltip("After how much time does mana regen and how much mana regen?")]
    [SerializeField]
    private float _manaRegenAmount = 10f, _regenTimer = 5f;

    public float _amountOfMana;

    private float timer;

    private void Start()
    {
        _amountOfMana = _maxMana;
        _manaBar.PlayerMana = (_maxMana / _maxMana) * 100;

        _spellType = 0;
        if (_spellIcon != null) _spellIcon.sprite = _spellIcons[_spellType];
    }

    private void LateUpdate()
    {
        if (_amountOfMana < _maxMana)
        {
            timer += Time.deltaTime;

            if (timer > _regenTimer)
            {
                _amountOfMana += _manaRegenAmount;
                _manaBar.PlayerMana = (_amountOfMana / _maxMana) * 100;
                timer = 0;
            }
        }
    }

    public void CastSpell(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_amountOfMana > 0) 
        {
            GameObject spawnedObj = Instantiate(_spell[_spellType], _castPoint.position, _castPoint.rotation);
            spawnedObj.GetComponent<Rigidbody>().AddForce(_castPoint.forward * _spellSpeed,ForceMode.Impulse);
             _amountOfMana -= _spell[_spellType].GetComponent<SpellController>().SpellData.amountOfMana;
            _manaBar.PlayerMana = (_amountOfMana / _maxMana) * 100;
            timer = 0;
        }
    }

    public void ResetMana()
    {
        _amountOfMana = _maxMana;
    }

    public void SetSpell(Component sender, object obj)
    {
        if (sender.transform.parent.gameObject.transform.parent.gameObject != gameObject) return;
        Spells spell = (Spells)(obj as Spells?);

        switch(spell)
        {
            case Spells.Default:
                _spellType = 0;
                break;
            case Spells.WaterBall:
                _spellType = 1;
                break;
            case Spells.FireBall:
                _spellType = 2;
                break;
            case Spells.AcidBall:
                _spellType = 3;
                break;
        }

        if (_spellIcon != null) _spellIcon.sprite = _spellIcons[_spellType];
    }
}
