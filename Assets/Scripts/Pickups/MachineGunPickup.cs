using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineGunPickup : Pickup, IDamageable
{
    [SerializeField] private float powerupTime;

    public override void OnPickup()
    {
        base.OnPickup();

        // Start powerup in gameManager
        GameManager.GetInstance().Countdown(powerupTime);
    }

    public void GetDamage(float damage)
    {
        OnPickup();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup();
            GameManager.GetInstance().scoreManager.IncrementScore();
        }
    }
}
