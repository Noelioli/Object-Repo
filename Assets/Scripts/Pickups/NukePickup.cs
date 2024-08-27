using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukePickup : Pickup, IDamageable
{
    public override void OnPickup()
    {
        base.OnPickup();

        // increase nukes
        GameManager.GetInstance().AddNuke();
    }

    public void DestroyAll()
    {
        GameManager.GetInstance().RemoveNuke();
        foreach (Enemy item in FindObjectsOfType(typeof(Enemy)))
        {
            item.Die();
            GameManager.GetInstance().scoreManager.IncrementScore();
        }
        foreach (Pickup item in FindObjectsOfType(typeof(Pickup)))
        {
            Destroy(item.gameObject);
        }
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
