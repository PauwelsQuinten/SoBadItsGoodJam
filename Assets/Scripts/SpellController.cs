using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 10;
    [SerializeField]
    public SpellData SpellData;

    private float _timer = 0;

    public GameObject Sender;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Sender) return;
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
                other.gameObject.GetComponent<PlayerHealth>().DoDamage(SpellData.amountOfDamage);
        }
        Destroy(gameObject);
    }
}
