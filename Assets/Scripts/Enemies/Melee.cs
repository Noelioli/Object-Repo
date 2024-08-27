using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Melee : Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime;

    [SerializeField] private GameObject[] deathEffects;
    [SerializeField] private GameObject[] bigEffects;
    private GameObject selectedEffect;

    private float timer = 0f;
    private float setSpeed = 0f;
    bool isBoss;
    int bossHealth;

    protected override void Start()
    {
        base.Start();
        isBoss = gameObject.GetComponent<Boss>() != null;
        bossHealth = GameManager.GetInstance().BossHealth;
        this.SetEnemyType(EnemyType.Melee);
        if (!isBoss)
            health = new Health(1, 0, 1);
        else
            health = new Health(bossHealth, 0, bossHealth);
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

    public override void Die()
    {
        if (isBoss)
        {
            selectedEffect = bigEffects[Random.Range(0, deathEffects.Length)];
            Instantiate(selectedEffect, transform.position, Quaternion.identity);
        }
        selectedEffect = deathEffects[Random.Range(0, deathEffects.Length)];
        Instantiate(selectedEffect, transform.position, Quaternion.identity);
        base.Die();
    }
}
