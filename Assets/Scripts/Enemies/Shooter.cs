using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject beamPrefab;

    private float timer = 1f;
    private float setSpeed = 0f;
    private bool hasFired;
    private GameObject beam; //Square, slightly opaque

    protected override void Start()
    {
        beam = Instantiate(beamPrefab);
        beam.SetActive(false);
        base.Start();
        this.SetEnemyType(EnemyType.Shooter);
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
            ActivateBeam(target.position);
            speed = 0f;
            if (!hasFired)
            {
                StartCoroutine(FiringCooldown());
            }
        }
        else
        {
            beam.SetActive(false);
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

    public void ActivateBeam(Vector2 player)
    {
        beam.SetActive(true);

        beam.gameObject.transform.position = ((target.position + transform.position) / 2f); //Midway between Enemy and Player
        
        player.x -= transform.position.x;
        player.y -= transform.position.y;

        float angle = (Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg) - 90f;

        beam.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle); // Angled to align ends

        beam.transform.localScale = new Vector3(0.2f, Vector2.Distance(transform.position, target.position), 1f); // Stretch to ensure length is correct
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
            weapon.Shoot(bulletPrefab, this, "Player");
        }
    }

    public void SetShooterEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
    }

    public override void Die()
    {
        Destroy(beam);
        base.Die();
    }
}
