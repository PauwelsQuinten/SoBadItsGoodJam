using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private GameEvent _playerDied;
    [SerializeField]
    private HealthBar _healthBar;

    private float _startingHealth = 100f;

    private void Start()
    {
        _healthBar._playerHealth = _startingHealth;
    }

    public void DoDamage(float amountOfDamage)
    {
        _healthBar._playerHealth -= amountOfDamage;

        CheckHealth();
    }

    public void ResetHealth()
    {
        _healthBar._playerHealth = _startingHealth;
    }

    private void CheckHealth()
    {
        if(_healthBar._playerHealth <= 0)
            _playerDied.Raise(this, gameObject);
    }
}
