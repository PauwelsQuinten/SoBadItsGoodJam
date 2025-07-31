using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Slider _manaSlider;

    public float PlayerMana {  get; set; }

    private void Awake()
    {
        _manaSlider = gameObject.GetComponent<Slider>();

        if (_manaSlider == null) Debug.LogWarning("No ManaBar Slider Found !");
    }

    private void Update()
    {
        if (_manaSlider.value != PlayerMana) 
            _manaSlider.value = PlayerMana;
    }
}
