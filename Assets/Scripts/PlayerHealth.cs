using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100f;

    public float Health;
    [SerializeField]
    private GameEvent _playerDied;
    [SerializeField]
    private HealthBar _healthBar;

    private float _startingHealth = 100f;

    private void Start()
    {
        Health = MaxHealth;
        _healthBar._playerHealth = _startingHealth;
    }

    public void DoDamage(float amountOfDamage)
    {
        Health -= amountOfDamage;
        _healthBar._playerHealth -= amountOfDamage;

        CheckHealth();
    }

    private void CheckHealth()
    {
        if(_healthBar._playerHealth <= 0) _playerDied.Raise(this, gameObject);
        Debug.Log($" {gameObject.name} died, this battle is over! ");
    }
}
