using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _healthSlider;
    private Slider _easeHealthSlider;

    public float _playerHealth {  get; set; }
    private float _lerpSpeed = 0.05f;

    private void Awake()
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();

        foreach (Slider sl in sliders)
        {
            if (sl.gameObject.name == "HealthBar") _healthSlider = sl;
            if (sl.gameObject.name == "EaseHealthBar") _easeHealthSlider = sl;
        }
        if (_healthSlider == null) Debug.LogWarning("No HealthBar Slider Found !");
        if (_easeHealthSlider == null) Debug.LogWarning("No EaseHealthBar Slider Found !");
    }

    private void Update()
    {
        if ( _healthSlider.value != _playerHealth) _healthSlider.value = _playerHealth;

        if (_healthSlider.value != _easeHealthSlider.value) 
            _easeHealthSlider.value = Mathf.Lerp(_easeHealthSlider.value, _playerHealth, _lerpSpeed);
    }
}
