using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private float angle;

    [SerializeField]
    GameObject healthBar;

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
    private float spiralRandomWait = 0.1f;

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
        randShootingOffset = Random.Range(100, 500) / 100;
        randMovementOffset = Random.Range(0, 400) / 100;

        //Setup for sprites flashing white on hit
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
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
            // CONTINUOUS
            if (shootingType == shootingTypes.Continuous)
            {
                GameObject prj = projectilePool.GetObject();
                prj.transform.position = hand.transform.position; // adjusts the projectiles starting position and rotation according to the enemy hand's position/rotation
                prj.transform.rotation = hand.transform.rotation;
                prj.SetActive(true);
                StartCoroutine(CanShoot());
            }

            // ARC & ARC TIGHT
            else if (shootingType == shootingTypes.Arc || shootingType == shootingTypes.ArcTight)
            {
                GameObject[] prj = new GameObject[numberOfShots];
                StartCoroutine(CanShoot());
                float angleIndex = 0;
                if (numberOfShots % 2f == 0f)
                {
                    angleIndex = numberOfShots / -2 + 0.5f;
                }
                else
                {
                    angleIndex = -Mathf.FloorToInt(numberOfShots / 2);
                }
                float targetAngle = Mathf.Atan2(player.transform.position.y - hand.transform.position.y, player.transform.position.x - hand.transform.position.x) * Mathf.Rad2Deg;

                if (targetAngle < 0)
                {
                    targetAngle = 360 + targetAngle;
                }

                for (int i = 0; i < numberOfShots; i++)
                {
                    float angl = distanceAngle * angleIndex;
                    prj[i] = projectilePool.GetObject();
                    var radians = (targetAngle + angl) * Mathf.Deg2Rad;

                    prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius + gameObject.transform.position;
                    if (shootingType == shootingTypes.Arc)
                    {
                        prj[i].transform.rotation = hand.transform.rotation * Quaternion.Euler(0, 0, angl);
                    }
                    else
                    {
                        prj[i].transform.rotation = hand.transform.rotation;
                    }
                    prj[i].SetActive(true);
                    angleIndex++;
                }

            }

            // CIRCLE
            else if (shootingType == shootingTypes.Circle)
            {
                GameObject[] prj = new GameObject[numberOfShots];
                StartCoroutine(CanShoot());

                float angleChange = 360 / numberOfShots;
                float startAngle = Mathf.Atan2(player.transform.position.y - hand.transform.position.y, player.transform.position.x - hand.transform.position.x) * Mathf.Rad2Deg;
                float incrementAngle = 0;

                if (startAngle < 0)
                {
                    startAngle = 360 + startAngle;
                }

                for (int i = 0; i < numberOfShots; i++)
                {
                    incrementAngle = angleChange*i;
                    prj[i] = projectilePool.GetObject();
                    var radians = (startAngle + incrementAngle) * Mathf.Deg2Rad;

                    prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius + gameObject.transform.position;
                    prj[i].transform.rotation = hand.transform.rotation * Quaternion.Euler(0, 0, incrementAngle);
                    prj[i].SetActive(true);
                }
            }

            // SPIRAL
            else if (shootingType == shootingTypes.Spiral)
            {
                StartCoroutine(CanShoot());

                StartCoroutine(SpiralWait());

                
            }

            // RANDOM
            else if (shootingType == shootingTypes.Random)
            {
                // TODO
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
        float startAngle = Mathf.Atan2(player.transform.position.y - hand.transform.position.y, player.transform.position.x - hand.transform.position.x) * Mathf.Rad2Deg;
        float incrementAngle = 0;

        if (startAngle < 0)
        {
            startAngle = 360 + startAngle;
        }

        for (int i = 0; i < Mathf.Floor(numberOfShots*numberOfSpins); i++)
        {
            
            incrementAngle = angleChange * i;

            prj[i] = projectilePool.GetObject();
            var radians = (startAngle + incrementAngle) * Mathf.Deg2Rad;

            prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius + gameObject.transform.position;
            prj[i].transform.rotation = hand.transform.rotation * Quaternion.Euler(0, 0, incrementAngle);
            prj[i].SetActive(true);

            yield return new WaitForSeconds(spiralRandomWait);
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
        angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
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
