using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 3;
    private int health;

    [SerializeField]
    GameObject healthBar;

    [SerializeField]
    private List<Sprite> healthbarSprites;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject spawnPoof;
    private float randMovementOffset;

    [SerializeField]
    private float movementVelocity = 3f;

    [SerializeField]
    private float stopTime = 2f;

    [SerializeField]
    private float allocatedWalkTime = 2f;
    [SerializeField]
    private float movementVariant = 0f;

    private bool canWalk = false;
    public bool facingRight = true;
    private Vector3 walkPosition = Vector3.zero;
    private bool firstTimeMove = true;
    private float angle;

    private AudioSource SFX;

    [SerializeField]
    private AudioClip hit;
    [SerializeField]
    private AudioClip death;


    private Vector2 collissionPoint;

    private void Start()
    {
        health = maxHealth;
        

        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");

        randMovementOffset = Random.Range(0, 200) / 100;

        SFX = GameObject.Find("SoundManager").transform.Find("SFXManager").GetComponent<AudioSource>();
        changeHealth(0);
    }

    public void changeMoveIntensity(int intensity)
    {
        movementVelocity *= intensity;
        stopTime /= intensity;
    }

    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            StartCoroutine(flashWhite());
        }
        health += amount;
        if (health <= 0)
        {
            GameObject spawn = Instantiate(spawnPoof, transform.position, Quaternion.identity);
            Destroy(spawn, 0.3f);
            SFX.PlayOneShot(death);
            Destroy(gameObject, 0.2f);
        }
        if (health >= 0)
        {
            SFX.PlayOneShot(hit);
            healthBar.GetComponent<SpriteRenderer>().sprite = healthbarSprites[Mathf.FloorToInt((health * 1f) / (maxHealth * 1f) * 10f)];
        }
    }
    public void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    void FixedUpdate()
    {
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

    IEnumerator flashWhite()
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("PlayerProjectile"))
        {
            collissionPoint = collision.contacts[0].point;
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
                float angle = 0f;
                angle = Random.Range(0, 360);

                // Changes facing direction according to relative player position.
                if (((angle >= 0 && angle <= 90) || (angle >= 270 && angle <= 360)) && !facingRight)
                {
                    Vector3 newScale = gameObject.transform.localScale;
                    newScale.x *= -1;
                    gameObject.transform.localScale = newScale;
                    facingRight = !facingRight;
                }
                else if (angle >= 90 && angle <= 270 && facingRight)
                {
                    Vector3 newScale = gameObject.transform.localScale;
                    newScale.x *= -1;
                    gameObject.transform.localScale = newScale;

                    facingRight = !facingRight;
                }

                float radians = angle * Mathf.Deg2Rad;
                Vector3 movePos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * 15;
                StartCoroutine(CanWalk());
                return movePos;
            }
        }
    }

    IEnumerator WaitToMove(float time)
    {
        yield return new WaitForSeconds(time);
        firstTimeMove = false;
    }

    IEnumerator CanWalk()
    {
        // Villager begins walking. After the allocated time has passed, their new destination is nulled. Then they wait for a while before
        // they can begin moving again.
        canWalk = true;
        float variant = Random.Range(-movementVariant, +movementVariant);
        yield return new WaitForSeconds(allocatedWalkTime + variant);
        walkPosition = transform.position;
        yield return new WaitForSeconds(stopTime);
        canWalk = false;
    }
}
