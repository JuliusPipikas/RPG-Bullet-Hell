using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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
    private float timeBetweenShots = .5f;

    [SerializeField]
    private int maxHealth = 10;
    private int health = 10;

    private Actions controls;
    private bool canShoot = true;
    private Camera main;
    private bool facingRight = true;

    [SerializeField]
    private GameObject healthBar;

    [SerializeField]
    private List<Sprite> healthbarSprites;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    private bool playerGotHit = false;

    private void Awake()
    {
        controls = new Actions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public bool getPlayerGotHit()
    {
        return playerGotHit;
    }

    public void setPlayerGotHit(bool b)
    {
        playerGotHit = b;
    }

    public void startGrace()
    {
        StartCoroutine(PlayerHitGrace());
    }

    public IEnumerator PlayerHitGrace()
    {
        yield return new WaitForSeconds(0.5f);
        setPlayerGotHit(false);
    }

    private void Start()
    {
        controls.Player.Shoot.performed += _ => PlayerShoot();
        main = Camera.main;
        //Setup for sprites flashing white on hit
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    private void PlayerShoot()
    {
        if (!canShoot) return;

        // Adjusts the direction of the shot according to current mouse position. Then adjusts the projectile starting position
        // and rotation according to the Player GameObject's position and rotation.
        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GameObject prj = projectilePool.GetObject();
        prj.transform.position = hand.transform.position;
        prj.transform.rotation = hand.transform.rotation;
        prj.SetActive(true);
        StartCoroutine(CanShoot());
    }

    IEnumerator CanShoot()
    {
        // Timer for shooting.
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position + new Vector3(0, 0, -10f);
        if(pos.y > 2.9) { pos.y = 2.9f; }
        else if(pos.y < -2.9) { pos.y = -2.9f; }
        if(pos.x > 1.2) { pos.x = 1.2f; }
        else if(pos.x < -1.2) { pos.x = -1.2f; }
        main.transform.position = pos;
        healthBar.transform.position = transform.position + new Vector3(0, 0.5f);

        // movement
        Vector2 pVec = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 movement = pVec * movementVelocity;

        transform.position += movement * Time.deltaTime;

        // movement forward tilt
        if (pVec.x > 0 && facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, -9);
        }
        else if (pVec.x < 0 && !facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 9);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // hand rotation & tracking
        Vector2 mouseScreenPosition = controls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = main.ScreenToWorldPoint(mouseScreenPosition);

        // Adjusts the target direction according to current facing direction
        Vector3 targetDirection = new Vector3();
        if (facingRight)
        {
            targetDirection = mouseWorldPosition - transform.position;
        }
        else
        {
            targetDirection = -mouseWorldPosition + transform.position;
        }

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        projectileDirection.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // player flipping
        if(mouseWorldPosition.x > transform.position.x && !facingRight)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            facingRight = !facingRight;
        }
        else if(mouseWorldPosition.x < transform.position.x && facingRight)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            facingRight = !facingRight;
        }
    }

    public void changeHealth(int amount)
    {
        if(amount < 0)
        {
            StartCoroutine(flashWhite());
        }
        health += amount;
        if (health >= 0)
        {
            healthBar.GetComponent<SpriteRenderer>().sprite = healthbarSprites[Mathf.FloorToInt((health * 1f) / (maxHealth * 1f) * 10f)];
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
