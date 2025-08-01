using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 10;
    [SerializeField]
    public SpellData SpellData;

    private float _timer = 0;


    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (other.gameObject.GetComponent<PlayerHealth>() != null)
            other.gameObject.GetComponent<PlayerHealth>().DoDamage(SpellData.amountOfDamage);

        Debug.Log($"Hit player: {other.gameObject.name}");

        Destroy(gameObject);
    }
}
