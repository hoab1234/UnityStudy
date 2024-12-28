using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour, IHealthObserver
{
    private Slider healthSlider;

    void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        FindObjectOfType<PlayerHealth>().RegisterObserver(this);
    }

    public void OnHealthChanged(float currentHealth)
    {
        Debug.Log($"UIHealthBar OnHealthChanged currentHealth : {currentHealth}");

        healthSlider.value = currentHealth;
    }
}