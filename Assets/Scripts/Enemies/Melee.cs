using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime;

    private float timer = 0f;
    private float setSpeed = 0f;

    protected override void Start()
    {
        base.Start();
        this.SetEnemyType(EnemyType.Melee);
        health = new Health(1,0,1);
        setSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();

        if (target == null)
            return;

        if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            speed = 0f;
            Attack(attackTime);
        }
        else
        {
            speed = setSpeed;
        }
    }

    public override void Attack(float interval)
    {
        if (timer < interval)
        {
            timer += interval;
        }
        else
        {
            timer = 0;
            target.GetComponent<IDamageable>().GetDamage(0.02f);
        }
    }

    public void SetMeleeEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
    }
}
