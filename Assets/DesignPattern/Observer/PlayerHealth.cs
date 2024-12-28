using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, ISubject
{
    private float currentHealth = 100f;
    public float MaxHealth = 100f;
    private List<IHealthObserver> observers = new List<IHealthObserver>();

    public Button hpUpButton;
    public Button hpDownButton;

    private void Awake()
    {
        hpUpButton.onClick.AddListener(() => TakeDamage(10));
        hpDownButton.onClick.AddListener(() => TakeDamage(-10));
    }

    public void RegisterObserver(IHealthObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IHealthObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"TakeDamage : {damage} currentHealth = {currentHealth + damage}");

        if (currentHealth + damage < 0 || currentHealth + damage > 100) 
            return;

        currentHealth += damage;
        Debug.Log("Notify");
        NotifyObservers();
    }
}