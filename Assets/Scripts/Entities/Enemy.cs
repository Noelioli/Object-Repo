using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PlayableObject
{
    private string playableObject_name;

    private EnemyType enemyType;
    protected Transform target;
    [SerializeField] protected float speed;

    protected virtual void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Destroy(this);
        }
    }

    protected virtual void Update()
    {
        if (target != null)
        {
            Move(target.position);
        }
        else
        {
            Move(speed);
        }
    }

    public void SetEnemyType(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    public EnemyType GetEnemyType()
    {
        return this.enemyType;
    }

    public override void Move(Vector2 direction, Vector2 target)
    {
        Debug.Log($"Move towards " + target);
    }

    public override void Move(Vector2 direction)
    {
        direction.x -= transform.position.x;
        direction.y -= transform.position.y;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public override void Move(float speed)
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public override void Shoot()
    {

    }

    public override void Attack(float interval)
    {
        Debug.Log($"Attacking with an Interval of {interval}");
    }

    public override void Die()
    {
        GameManager.GetInstance().NotifyDeath(this);
        Destroy(gameObject);
    }

    public override void GetDamage(float damage)
    {
        health.DeductHealth(damage);
        if (health.GetHealth() <= 0)
            Die();
    }
}
