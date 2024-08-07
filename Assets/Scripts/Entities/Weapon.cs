using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private string name;
    private float damage;
    private float bulletSpeed;

    public Weapon(string _name, float _damage, float _bulletSpeed)
    {
        name = _name; 
        damage = _damage;
        bulletSpeed = _bulletSpeed;
    }

    public Weapon() { }

    public void Shoot(Bullet _bullet, PlayableObject _player, string _targetTag, float _timeToDie = 5f, float _spread = 0f)
    {
        Bullet tempBullet = GameObject.Instantiate(_bullet, _player.transform.position, _player.transform.rotation * Quaternion.Euler(0,0,Random.Range(-_spread, _spread))); //Quaternions make me want to die.
        tempBullet.SetBullet(damage, _targetTag, bulletSpeed);
        GameObject.Destroy(tempBullet, _timeToDie);
    }

    public float GetDamage()
    {
        return damage;
    }
}
