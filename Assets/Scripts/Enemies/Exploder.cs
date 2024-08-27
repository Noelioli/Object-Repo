using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : Enemy
{
    private float explosiveForce = 50f;
    private float normalSpeed;

    [SerializeField] private GameObject[] attackEffects;
    [SerializeField] private GameObject[] deathEffects;
    private GameObject selectedEffect;

    bool isBoss;
    int bossHealth;

    protected override void Start()
    {
        base.Start();
        isBoss = gameObject.GetComponent<Boss>() != null;
        bossHealth = GameManager.GetInstance().BossHealth;
        this.SetEnemyType(EnemyType.Exploder);
        if (!isBoss)
            health = new Health(1, 0, 1);
        else
            health = new Health(bossHealth, 0, bossHealth);
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
        selectedEffect = attackEffects[Random.Range(0, attackEffects.Length)];
        Instantiate(selectedEffect, transform.position, Quaternion.identity);
        Die();
    }

    public override void Die()
    {
        if (isBoss)
        {
            selectedEffect = attackEffects[Random.Range(0, deathEffects.Length)];
            Instantiate(selectedEffect, transform.position, Quaternion.identity);
        }
        selectedEffect = deathEffects[Random.Range(0, deathEffects.Length)];
        Instantiate(selectedEffect, transform.position, Quaternion.identity);
        base.Die();
    }
}
