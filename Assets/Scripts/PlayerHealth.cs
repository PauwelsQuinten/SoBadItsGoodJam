using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100f;

    private float _health;
    private GameEvent _playerDied;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void DoDamage(float amountOfDamage)
    {
        _health -= amountOfDamage;
        Debug.Log($" {gameObject.name} has {_health} health left");

        CheckHealth();
    }

    private void CheckHealth()
    {
        if( _health <= 0) _playerDied.Raise(this, gameObject);
        Debug.Log($" {gameObject.name} died, this battle is over! ");
    }
}
