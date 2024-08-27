using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    int bossHealth;

    // Start is called before the first frame update
    protected override void Start()
    {
        bossHealth = GameManager.GetInstance().BossHealth;
        transform.localScale = Vector3.one;
        health = new Health(bossHealth,0,3);
    }
}
