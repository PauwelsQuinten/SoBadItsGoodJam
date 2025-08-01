using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100f;

    public float Health;
    [SerializeField]
    private GameEvent _playerDied;

    private void Start()
    {
        Health = MaxHealth;
    }

    public void DoDamage(float amountOfDamage)
    {
        Health -= amountOfDamage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if( Health <= 0) _playerDied.Raise(this, gameObject);
    }
}
