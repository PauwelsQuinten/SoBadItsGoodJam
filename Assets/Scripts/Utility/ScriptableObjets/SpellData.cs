using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spells/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string spellName;
    public float amountOfDamage;
    public float amountOfMana;
}
