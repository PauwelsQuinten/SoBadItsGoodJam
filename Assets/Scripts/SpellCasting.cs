using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting : MonoBehaviour
{
    [Header("Every spell needs a rigidbody and a Collider!!!")]
    [SerializeField]
    private GameObject[] _spell;
    [SerializeField]
    private Transform _castPoint;
    [SerializeField]
    private float _spellSpeed = 5f;
    [SerializeField]
    public int _spellType = 0; //Later on change to a property 

    

    public void CastSpell(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        GameObject spawnedObj = Instantiate(_spell[_spellType], _castPoint.position, Quaternion.Euler(_castPoint.forward));
        spawnedObj.GetComponent<Rigidbody>().AddForce(_castPoint.forward * _spellSpeed, ForceMode.Impulse);

        Debug.Log($"Spell \" {_spell[_spellType].name} \" casted");
    }
}
