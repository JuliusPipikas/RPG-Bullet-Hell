using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private int damage = 1;

    private Pooler pool;

    private void Start()
    {
        pool = transform.parent.GetComponent<Pooler>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyProjectileAfterTime());
    }

    IEnumerator DestroyProjectileAfterTime()
    {
        yield return new WaitForSeconds(3f);
        pool.ReturnObject(gameObject);
    }
    
    void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

   /* private void OnTriggerEnter(Collider other)
    {
        pool.ReturnObject(gameObject);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.collider.gameObject.GetComponent<PlayerController>().changeHealth(-damage);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") && collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.collider.gameObject.GetComponent<EnemyController>().changeHealth(-damage);
        }
        pool.ReturnObject(gameObject);
    }
}
