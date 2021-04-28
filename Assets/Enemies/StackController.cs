using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackController : MonoBehaviour
{
    [SerializeField]
    private Transform projectileDirection;
    [SerializeField]
    private List<GameObject> hands;
    [SerializeField]
    private float movementVelocity = 3f;

    [SerializeField]
    private bool moveTowardsPlayer = false;

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
    private float angle;
    private bool relocating = false;
    private Vector2 collissionPoint;

    [SerializeField]
    GameObject healthBar;

    [SerializeField]
    private float healthBarOffset = 0.25f;

    [SerializeField]
    private List<Sprite> healthbarSprites;

    [SerializeField]
    private float timeBetweenShots = 1f;

    enum shootingTypes
    {
        Continuous,
        Arc,
        ArcTight,
        Circle,
        Spiral,
        Random
    }

    [SerializeField]
    shootingTypes shootingType = shootingTypes.Continuous;

    [SerializeField]
    private int numberOfShots = 1;

    [SerializeField]
    private float distanceAngle = 16f;

    [SerializeField]
    private float radius = 0.1f;

    [SerializeField]
    private float numberOfSpins = 1f;

    [SerializeField]
    private float spiralWaitBetweenShots = 0.1f;

    public Vector3 SpawnPoofOffset = Vector3.zero;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject spawnPoof;
    private AudioSource SFX;
    [SerializeField]
    private AudioClip hit;
    [SerializeField]
    private AudioClip death;
    [SerializeField]
    private AudioClip spawn;
    [SerializeField]
    private AudioClip rage;
    [SerializeField]
    private AudioClip shoot;
    public GameObject shadow;
    public GameObject instantiatedShadow;

    private void Awake()
    {
        health = maxHealth;
        
        player = GameObject.FindGameObjectWithTag("Player");
        randShootingOffset = Random.Range(200, 500) / 100;
        randMovementOffset = Random.Range(0, 200) / 100;

        //Setup for sprites flashing white on hit
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");


        SFX = GameObject.Find("SoundManager").transform.Find("SFXManager").GetComponent<AudioSource>();
        changeHealth(0);
        SFX.PlayOneShot(spawn);

        Vector3 shadowPos = new Vector3(0, -myRenderer.bounds.size.y+0.1f, 0) + gameObject.transform.parent.transform.localPosition;
        shadow.GetComponent<SpriteRenderer>().sprite = myRenderer.sprite;
        instantiatedShadow = Instantiate(shadow, shadowPos, Quaternion.identity, gameObject.transform);
        instantiatedShadow.transform.rotation = new Quaternion(0, 0, 180, 0);
    }

    private void Shoot()
    {
        // Projectile automatically aims for player.
        if (!canShoot)
        {
            return;
        }
        if (firstTimeShoot)
        {
            StartCoroutine(WaitToShoot(randShootingOffset));
        }
        else
        {
            // SPIRAL
            if (health > maxHealth/2)
            {
                StartCoroutine(CanShoot());

                StartCoroutine(SpiralWait());

                
            }

            // RANDOM
            else
            {
                StartCoroutine(CanShoot());

                StartCoroutine(RandomWait());
            }
        }
    }

    public void setProjectilePool(Pooler pp)
    {
        projectilePool = pp;
    }

    IEnumerator SpiralWait()
    {
        GameObject[] prj = new GameObject[(int)Mathf.Floor(numberOfShots * numberOfSpins)];

        float angleChange = 360 / numberOfShots;
        float startAngle = 0;
        float incrementAngle = 0;

        for (int i = 0; i < Mathf.Floor(numberOfShots*numberOfSpins); i++)
        {
            
            incrementAngle = angleChange * i;

            prj[i] = projectilePool.GetObject();
            var radians = (startAngle + incrementAngle) * Mathf.Deg2Rad;

            prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius + gameObject.transform.position;
            prj[i].transform.rotation = Quaternion.Euler(0, 0, incrementAngle);
            prj[i].SetActive(true);
            SFX.PlayOneShot(shoot);

            yield return new WaitForSeconds(spiralWaitBetweenShots);
        }
    }

    IEnumerator RandomWait()
    {
        GameObject[] prj = new GameObject[(int)Mathf.Floor(numberOfShots * numberOfSpins)];

        float rand_angle;

        for (int i = 0; i < Mathf.Floor(numberOfShots * numberOfSpins); i++)
        {
            prj[i] = projectilePool.GetObject();
            rand_angle = Random.Range(0, 361);
            var radians = (rand_angle) * Mathf.Deg2Rad;

            float rand_height = Random.Range(-0.5f, 0.5f);

            prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius + gameObject.transform.position + new Vector3(0, rand_height, 0);
            prj[i].transform.rotation = Quaternion.Euler(0, 0, rand_angle);
            prj[i].SetActive(true);
            SFX.PlayOneShot(shoot);

            yield return new WaitForSeconds(spiralWaitBetweenShots/2);
        }
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
        yield return new WaitForSeconds(timeBetweenShots);
        
        canShoot = true;
    }

    void FixedUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, healthBarOffset);

        Shoot();
        
        // Sets the target position of the projectile to the player's current position/
        Vector3 targetPosition = player.transform.position;

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

        int rand_angle;

        if (health >= maxHealth / 2)
        {
            rand_angle = Random.Range(0, 4);
        }
        else
        {
            rand_angle = Random.Range(5, 9);
        }
        for (int i = 0; i< hands.Count; i++)
        {
            
            hands[i].transform.parent.transform.rotation = hands[i].transform.parent.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, rand_angle));
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
            collissionPoint = collision.contacts[0].point;
            canWalk = false;
            StopCoroutine(CanWalk());
            walkPosition = transform.position;
            if (moveTowardsPlayer)
            {
                relocating = true;
            }
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
                if (!moveTowardsPlayer || relocating)
                {
                    float angle = 0f;
                    if (!relocating)
                    {
                        angle = Random.Range(0, 360);
                    }
                    else
                    {
                        float hit_angle = Mathf.Atan2(transform.position.y - collissionPoint.y, transform.position.x - collissionPoint.x) * Mathf.Rad2Deg;

                        if (hit_angle < 0)
                        {
                            hit_angle = 360 + hit_angle;
                        }

                        int dir = Random.Range(0, 2);
                        if(dir == 0)
                        {
                            angle = Random.Range(210, 290);
                        }
                        else
                        {
                            angle = Random.Range(70, 150);
                        }

                        angle += hit_angle;

                        if(angle > 360)
                        {
                            angle -= 360;
                        }

                    }
                    float radians = angle * Mathf.Deg2Rad;
                    Vector3 movePos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * 15;
                    StartCoroutine(CanWalk());
                    return movePos;
                }
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
        relocating = false;
        canWalk = false;
    }

    private bool firstTimeRage = true;
    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            StartCoroutine(flashWhite());
        }
        health += amount;
        if(!firstTimeRage && health > maxHealth / 2)
        {
            firstTimeRage = false;
            SFX.PlayOneShot(rage);
        }
        if (health <= 0)
        {
            GameObject spawn = Instantiate(spawnPoof, transform.position, Quaternion.identity);
            Destroy(spawn, 0.3f);
            SFX.PlayOneShot(death);

            Destroy(gameObject.transform.parent.gameObject, 0.2f);
        }
        if (health >= 0)
        {
            int t = Mathf.FloorToInt((health * 1f) / (maxHealth * 1f) * 10f);
            if(t == 0)
            {
                t = 1;
            }
            healthBar.GetComponent<SpriteRenderer>().sprite = healthbarSprites[t];
            SFX.PlayOneShot(hit);
        }
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
