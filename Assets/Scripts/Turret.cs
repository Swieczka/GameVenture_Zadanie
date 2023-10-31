using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject cannonPoint;
    float shootingForce=0;
    float shootingCooldown;
    float shootingCooldownResetTime=0;
    public bool isActive;
    void Start()
    {
        shootingCooldown = 0;
        //isActive = false;
    }

    void Update()
    {
        if (!isActive) return;
        if(Time.time >= shootingCooldown)
        {
            shootingCooldown = Time.time + shootingCooldownResetTime;
            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject _bullet = GameObject.Instantiate(bullet, cannonPoint.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
        _bullet.GetComponent<Bullet>().speed = shootingForce;
    }
    public void SetTurret(bool active, float force, float cooldoown)
    {
        isActive = active;
        shootingCooldownResetTime = cooldoown;
        shootingForce = force;
    }
}
