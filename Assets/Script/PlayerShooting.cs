using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // 子弹预制体
    public Transform bulletSpawnPoint; // 子弹生成位置
    public float bulletSpeed = 20f; // 子弹速度
    public float shootCooldown = 0.5f; // 射击冷却时间

    private Camera mainCamera;
    private float shootTimer;

    void Start()
    {
        mainCamera = Camera.main;
        shootTimer = 0f;
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

        
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        Vector3 shootDirection;

        if (Physics.Raycast(ray, out hit))
        {
            
            shootDirection = (hit.point - bulletSpawnPoint.position).normalized;
        }
        else
        {
            
            shootDirection = ray.direction;
        }

       
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = shootDirection * bulletSpeed;
        }

        
        Destroy(bullet, 5f);
    }
}


