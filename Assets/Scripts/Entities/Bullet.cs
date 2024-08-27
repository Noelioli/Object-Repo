using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private bool isPlayer;

    private string targetTag;
    
    public void SetBullet(float _damage, string _targetTag, float _speed = 15)
    {
        this.damage = _damage;
        this.targetTag = _targetTag;
        this.speed = _speed;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void Damage(IDamageable damageable)
    {
        if (damageable != null)
        {
            damageable.GetDamage(damage);
            if (isPlayer)
                GameManager.GetInstance().scoreManager.IncrementScore();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        if (!collision.gameObject.CompareTag(targetTag))
            return;

        //Interface
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        Damage(damageable);
    }
}
