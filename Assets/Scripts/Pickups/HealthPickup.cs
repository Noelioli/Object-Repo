using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup, IDamageable
{
    [SerializeField] private float healthMin;
    [SerializeField] private float healthMax;

    public override void OnPickup()
    {
        base.OnPickup();

        // increase health
        float health = Random.Range(healthMin, healthMax);

        var player = GameManager.GetInstance().GetPlayer();

        player.health.AddHealth(health);
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
