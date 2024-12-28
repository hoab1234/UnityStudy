using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour, IHealthObserver
{
    private Image vignetteEffect;
    float playerMaxHP;

    void Start()
    {
        vignetteEffect = GetComponentInChildren<Image>();
        vignetteEffect.enabled = false;

        var playerHP = FindObjectOfType<PlayerHealth>();
        playerHP.RegisterObserver(this);
        playerMaxHP = playerHP.MaxHealth;
    }

    public void OnHealthChanged(float currentHealth)
    {
        Debug.Log($" ScreenEffect OnHealthChanged currentHealth : {currentHealth}");

        if (currentHealth <= playerMaxHP * 0.3f)
        {
            vignetteEffect.enabled = true;
        }
        else
        {
            vignetteEffect.enabled = false;
        }
    }
}