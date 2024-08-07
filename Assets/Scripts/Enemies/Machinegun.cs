using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime;
    [SerializeField] private Bullet bulletPrefab;

    private float timer = 0.2f;
    private float setSpeed = 0f;
    private bool hasFired;

    protected override void Start()
    {
        base.Start();
        this.SetEnemyType(EnemyType.Machinegun);
        health = new Health(1, 0, 1);
        setSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();

        if (target == null)
            return;

        LookTowardsPlayer(target.position);

        if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            speed = 0f;
            if (!hasFired)
            {
                StartCoroutine(FiringCooldown());
            }
        }
        else
        {
            speed = setSpeed;
        }
    }

    IEnumerator FiringCooldown()
    {
        hasFired = true;
        yield return new WaitForSeconds(timer);
        Attack(attackTime);
        hasFired = false;
    }

    private void LookTowardsPlayer(Vector2 player)
    {
        player.x -= transform.position.x;
        player.y -= transform.position.y;

        float angle = (Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
            weapon.Shoot(bulletPrefab, this, "Player", 5f, 25f);
        }
    }

    public void SetMachinegunEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
    }
}
