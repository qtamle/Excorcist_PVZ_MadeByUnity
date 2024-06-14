﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutCanon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float cooldownTime = 10f; 
    private bool canShoot = true; 
    public LayerMask targetLayer; 

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Ready(true); 
    }

    void Update()
    {
        if (canShoot)
        {
            Ready(true); 
        }

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                StartCoroutine(HandleShooting());
            }
        }
    }

    IEnumerator HandleShooting()
    {
        canShoot = false;
        Fire(true);
        yield return new WaitForSeconds(0.5f); 

        Shoot();
        Stop(true);
        yield return new WaitForSeconds(1.3f);

        Idle(true);
        yield return StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
        Ready(true); // Return to Ready state after cooldown
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10f, 0f); // Shoot the bullet to the right
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            Destroy(collision.gameObject); // Destroy the object when it collides with the specified LayerMask
        }
    }

    private void Idle(bool idle)
    {
        if (animator != null)
        {
            animator.SetBool("idle", idle);
            if (idle)
            {
                animator.SetBool("ready", false);
                animator.SetBool("fire", false);
                animator.SetBool("stop", false);
            }
        }
    }

    private void Ready(bool ready)
    {
        if (animator != null)
        {
            animator.SetBool("ready", ready);
            if (ready)
            {
                animator.SetBool("idle", false);
                animator.SetBool("fire", false);
                animator.SetBool("stop", false);
            }
        }
    }

    private void Fire(bool fire)
    {
        if (animator != null)
        {
            animator.SetBool("fire", fire);
            if (fire)
            {
                animator.SetBool("ready", false);
                animator.SetBool("idle", false);
            }
        }
    }

    private void Stop(bool stop)
    {
        if (animator != null)
        {
            animator.SetBool("stop", stop);
            if (stop)
            {
                animator.SetBool("fire", false);
                animator.SetBool("idle", false);
            }
        }
    }
}
