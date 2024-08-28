using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayableObject
{
    [SerializeField] private float speed;

    [SerializeField] private float weaponDamage = 1f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioClip bulletSound;

    public Action OnDeath;

    private Camera mainCamera;
    private Rigidbody2D playerRB;
    //private Vector3 direction;

    private void Awake()
    {
        //health = new Health(1000f, 100f, 1000);
        health = new Health(100f, 0.5f, 100);
        mainCamera = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();

        weapon = new Weapon("Player Weapon", weaponDamage, bulletSpeed);
    }

    private void Update()
    {
        health.RegenHealth();
    }

    /// <summary>
    /// Moves player's Rigidbody towards direction, and points player at target direction by rotating z axis.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="target"></param>
    public override void Move(Vector2 direction, Vector2 target)
    {
        playerRB.velocity = direction * speed * Time.deltaTime;

        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        target.x -= playerScreenPos.x;
        target.y -= playerScreenPos.y;

        float angle = (Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void Attack(float interval)
    {

    }

    public override void Shoot()
    {
        //Debug.Log("Shooting");
        playerSource.PlayOneShot(bulletSound);
        weapon.Shoot(bulletPrefab, this, "Enemy");
    }

    public override void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public override void GetDamage(float damage)
    {
        health.DeductHealth(damage);

        if (health.GetHealth() <= 0)
        {
            Die();
        }
    }
}
