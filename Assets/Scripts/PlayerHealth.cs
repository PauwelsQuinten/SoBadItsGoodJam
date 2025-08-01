using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private HealthBar _healthBar;

    private float _startingHealth = 100f;
    private GameEvent _playerDied;

    private void Start()
    {
        _healthBar._playerHealth = _startingHealth;
    }

    public void DoDamage(float amountOfDamage)
    {
        _healthBar._playerHealth -= amountOfDamage;
        Debug.Log($" {gameObject.name} has {_startingHealth} health left");

        CheckHealth();
    }

    private void CheckHealth()
    {
        if(_healthBar._playerHealth <= 0) _playerDied.Raise(this, gameObject);
        Debug.Log($" {gameObject.name} died, this battle is over! ");
    }
}
