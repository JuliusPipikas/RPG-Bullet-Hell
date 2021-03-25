using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Transform projectileDirection;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private float movementVelocity = 3f;

    [SerializeField]
    private Pooler projectilePool;

    [SerializeField]
    private float stopTime = 2f;

    [SerializeField]
    private float allocatedWalkTime = 2f;

    private float randMovementOffset;
    private float randShootingOffset;
    [SerializeField]
    private float movementVariant = 0f;

    [SerializeField]
    private int maxHealth = 10;
    private int health;
    private bool canShoot = true;
    private bool canWalk = false;
    public bool facingRight = true;
    private GameObject player;
    private Vector3 walkPosition = Vector3.zero;
    private bool firstTimeShoot = true;
    private bool firstTimeMove = true;

    [SerializeField]
    GameObject healthBar;

    public Vector3 SpawnPoofOffset = Vector3.zero;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject spawnPoof;

    private void Awake()
    {
        health = maxHealth;
        changeHealth(0);
        player = GameObject.FindGameObjectWithTag("Player");
        randShootingOffset = Random.Range(100, 400) / 100;
        randMovementOffset = Random.Range(0, 300) / 100;

        //Setup for sprites flashing white on hit
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    private void Shoot()
    {
        // Projectile automatically aims for player.
        // TODO: Create geometrical shooting patterns for non-standard enemies.
        if (!canShoot) return;
        if (firstTimeShoot)
        {
            StartCoroutine(WaitToShoot(randShootingOffset));
        }
        else
        {
            GameObject prj = projectilePool.GetObject();
            prj.transform.position = hand.transform.position; // adjusts the projectiles starting position and rotation according to the enemy hand's position/rotation
            prj.transform.rotation = hand.transform.rotation;
            prj.SetActive(true);
            StartCoroutine(CanShoot());
        }
    }

    public void setProjectilePool(Pooler pp)
    {
        projectilePool = pp;
    }
    IEnumerator WaitToShoot(float time)
    {
        yield return new WaitForSeconds(time);
        firstTimeShoot = false;
    }

    IEnumerator WaitToMove(float time)
    {
        yield return new WaitForSeconds(time);
        firstTimeMove = false;
    }
    IEnumerator CanShoot()
    {
        // Time for shooting
        canShoot = false;
        yield return new WaitForSeconds(2f);
        canShoot = true;
    }

    void FixedUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 0.3f);
        Shoot();
        // Sets the target position of the projectile to the player's current position/
        Vector3 targetPosition = player.transform.position;

        // Calculates the target (player) direction according to current facing.
        Vector3 targetDirection;
        if (facingRight)
        {
            targetDirection = targetPosition - transform.position;
        }
        else
        {
            targetDirection = -targetPosition + transform.position;
        }

        // Adjusts projectile rotation according to the target (player) location and relative angle.
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        projectileDirection.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Changes facing direction according to relative player position.
        if (targetPosition.x > transform.position.x && !facingRight)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            facingRight = !facingRight;
        }
        else if (targetPosition.x < transform.position.x && facingRight)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            
            facingRight = !facingRight;
        }


        // Movement
        if (canWalk)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walkPosition, movementVelocity / 100);

        }
        else
        {
            canWalk = false;
            walkPosition = FindWalkPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("PlayerProjectile"))
        {
            canWalk = false;
            StopCoroutine(CanWalk());
            walkPosition = transform.position;
        }
    }

    private Vector3 FindWalkPosition()
    {
        if (firstTimeMove)
        {
            StartCoroutine(WaitToMove(randMovementOffset));
            return gameObject.transform.position;
        }
        else
        {
            // Get random angle and get a point far away in that direction. The Enemy does not move to a specific point, but rather
            // in a direction for a given time "allocatedWalkTime". Then they pause for a time of "stopTime" ("IEnumerator CanWalk()").
            while (true)
            {
                float angle = Random.Range(0, 360);
                float radians = angle * Mathf.Deg2Rad;
                Vector3 movePos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * 15;
                StartCoroutine(CanWalk());
                return movePos;
            }
        }
    }

    IEnumerator CanWalk()
    {
        // Enemy begins walking. After the allocated time has passed, their new destination is nulled. Then they wait for a while before
        // they can begin moving again.
        canWalk = true;
        float variant = Random.Range(-movementVariant, +movementVariant);
        yield return new WaitForSeconds(allocatedWalkTime + variant);
        walkPosition = transform.position;
        yield return new WaitForSeconds(stopTime);
        canWalk = false;
    }

    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            StartCoroutine(flashWhite());
        }
        health += amount;
        if(health <= 0)
        {
            GameObject spawn = Instantiate(spawnPoof, transform.position, Quaternion.identity);
            Destroy(spawn, 0.3f);
            Destroy(gameObject.transform.parent.gameObject, 0.2f);
        }
        healthBar.GetComponent<TextMesh>().text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    IEnumerator flashWhite()
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }
}
