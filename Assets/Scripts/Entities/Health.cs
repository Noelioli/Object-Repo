using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private float currentHealth;
    private float maxHealth;
    private float healthRegenRate;

    public Action<float> OnHealUpdate;

    public Health() { }

    public Health(float _maxHealth, float _healthRegenRate, float _currentHealth = 100)
    {
        maxHealth = _maxHealth;
        currentHealth = _currentHealth;
        healthRegenRate = _healthRegenRate;

        OnHealUpdate?.Invoke(currentHealth);
    }

    public void SetHealth(float value)
    {
        if (value > maxHealth || value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        currentHealth = value;
    }

    public void RegenHealth()
    {
        AddHealth(healthRegenRate * Time.deltaTime);
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + value);
        OnHealUpdate?.Invoke(currentHealth);
    }

    public void DeductHealth(float value)
    {
        currentHealth = Mathf.Max(0, currentHealth - value);
        OnHealUpdate?.Invoke(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
