using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float projectileTime = 3f;

    [SerializeField]
    private GameObject spawnPoof;

    private Pooler pool;

    private void Start()
    {
        pool = transform.parent.GetComponent<Pooler>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyProjectileAfterTime());
        if (gameObject.GetComponent<BoxCollider2D>())
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (gameObject.GetComponent<CircleCollider2D>())
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        StartCoroutine(EnableCollirAfterTime());
    }

    IEnumerator DestroyProjectileAfterTime()
    {
        yield return new WaitForSeconds(projectileTime);
        pool.ReturnObject(gameObject);
    }

    IEnumerator EnableCollirAfterTime()
    {
        yield return new WaitForSeconds(0.1f);
        if (gameObject.GetComponent<BoxCollider2D>())
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (gameObject.GetComponent<CircleCollider2D>())
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!collision.collider.gameObject.GetComponent<PlayerController>().getPlayerGotHit())
            {
                GameObject.Find("Player").GetComponent<PlayerController>().startGrace();
                collision.collider.gameObject.GetComponent<PlayerController>().setPlayerGotHit(true);
                collision.collider.gameObject.GetComponent<PlayerController>().changeHealth(-damage);
            }
        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") && collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.collider.gameObject.GetComponent<EnemyController>().changeHealth(-damage);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") && collision.collider.gameObject.layer == LayerMask.NameToLayer("EnemyStack"))
        {
            collision.collider.gameObject.GetComponent<StackController>().changeHealth(-damage);
        }
        GameObject spawn = Instantiate(spawnPoof, collision.contacts[0].point, transform.rotation*spawnPoof.transform.rotation);
        Destroy(spawn, 0.1f);
        pool.ReturnObject(gameObject);
    }
}
