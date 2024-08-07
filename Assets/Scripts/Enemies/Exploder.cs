using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : Enemy
{
    private float explosiveForce = 50f;
    private float normalSpeed;

    protected override void Start()
    {
        base.Start();
        this.SetEnemyType(EnemyType.Exploder);
        health = new Health(1, 0, 1);
        normalSpeed = speed;
    }
    protected override void Update()
    {
        base.Update();

        if (target == null)
            return;

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Explode(explosiveForce);
        }
    }

    private void Explode(float explosiveForce)
    {
        target.GetComponent<IDamageable>().GetDamage(explosiveForce);
        Die();
    }
}
