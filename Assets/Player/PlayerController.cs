using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform projectileDirection;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject aura;
    [SerializeField]
    private float movementVelocity = 3f;

    [SerializeField]
    private Pooler projectilePool1;
    [SerializeField]
    private Pooler projectilePool2;
    [SerializeField]
    private Pooler projectilePool3;

    private Pooler projectilePool;

    [SerializeField]
    private float timeBetweenShots = .5f;

    [SerializeField]
    private int maxHealth = 10;
    private int health = 10;

    private Actions controls;
    private bool canShoot = true;
    public bool canSwap = true;
    private bool canBurst = false;
    private Camera main;
    private bool facingRight = true;

    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject swapBar;

    [SerializeField]
    private List<Sprite> healthbarSprites;
    [SerializeField]
    private List<Sprite> swapbarSprites;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    private bool playerGotHit = false;

    [SerializeField]
    private GameObject handForTexture;

    [SerializeField]
    private List<Sprite> weaponTextures;

    private int currentWeapon = 0;

    private void Awake()
    {
        controls = new Actions();
        projectilePool = projectilePool1;
    }

    public int getHealth()
    {
        return health;
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

        if (currentWeapon == 0 || currentWeapon == 3)
        {
            // Adjusts the direction of the shot according to current mouse position. Then adjusts the projectile starting position
            // and rotation according to the Player GameObject's position and rotation.
            //Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
            //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            GameObject prj = projectilePool.GetObject();
            prj.transform.position = hand.transform.position;
            prj.transform.rotation = hand.transform.rotation;
            if(currentWeapon == 3)
            {
                int textRand = Random.Range(0, 5);
                if(textRand == 0)
                {
                    prj.GetComponentInChildren<TextMesh>().text = "Persuasion!";
                }
                else if (textRand == 1)
                {
                    prj.GetComponentInChildren<TextMesh>().text = "Deception!";
                }
                else if (textRand == 2)
                {
                    prj.GetComponentInChildren<TextMesh>().text = "Intimidation!";
                }
                else if (textRand == 1)
                {
                    prj.GetComponentInChildren<TextMesh>().text = "Insight!";
                }
            }
            prj.SetActive(true);
            StartCoroutine(CanShoot());
        }
        else if(currentWeapon == 1)
        {
            GameObject[] prj = new GameObject[7];
            StartCoroutine(CanShoot());
            float angleIndex = -Mathf.FloorToInt(7 / 2);

            Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            float targetAngle = Mathf.Atan2(mousePosition.y - hand.transform.position.y, mousePosition.x - hand.transform.position.x) * Mathf.Rad2Deg;


            if (targetAngle < 0)
            {
                targetAngle = 360 + targetAngle;
            }

            for (int i = 0; i < 7; i++)
            {
                float angl = 30 * angleIndex;
                prj[i] = projectilePool.GetObject();
                var radians = (targetAngle + angl) * Mathf.Deg2Rad;

                prj[i].transform.position = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * 0.5f + gameObject.transform.position;
                prj[i].transform.rotation = hand.transform.rotation;
                prj[i].SetActive(true);
                angleIndex++;
            }

        }
    }

    IEnumerator CanShoot()
    {
        // Timer for shooting.
        canShoot = false;
        if(currentWeapon == 1)
        {
            handForTexture.GetComponent<Light2D>().enabled = false;
        }
        yield return new WaitForSeconds(timeBetweenShots);
        if (currentWeapon == 1)
        {
            handForTexture.GetComponent<Light2D>().enabled = true;
        }
        canShoot = true;
    }

    private void Update()
    {
        if (canSwap)
        {
            // Weapon swaps
            if (controls.Player.SwapWeapons1.triggered)
            {
                SwapWeapon(1);
            }
            else if (controls.Player.SwapWeapons2.triggered)
            {
                SwapWeapon(2);
            }
            else if (controls.Player.SwapWeapons3.triggered)
            {
                SwapWeapon(3);
            }
            else if (controls.Player.Next.triggered)
            {
                SwapWeapon(4);
            }
            else if (controls.Player.Previous.triggered)
            {
                SwapWeapon(5);
            }
        }
    }

    void FixedUpdate()
    {

        Burst();

        Vector3 pos = transform.position + new Vector3(0, 0, -10f);
        if(pos.y > 2.9) { pos.y = 2.9f; }
        else if(pos.y < -2.9) { pos.y = -2.9f; }
        if(pos.x > 1.2) { pos.x = 1.2f; }
        else if(pos.x < -1.2) { pos.x = -1.2f; }
        main.transform.position = pos;
        healthBar.transform.position = transform.position + new Vector3(0, 0.5f);
        swapBar.transform.position = transform.position + new Vector3(0, 0.42f);
        aura.transform.position = transform.position + new Vector3(0, -0.25f);

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

    public void SwapWeapon(int i)
    {
        if(currentWeapon == 6) {
            if (i == 6)
            {
                SwapToStaff();
                handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[0];
                currentWeapon = 0;
            }
        }
        else if(i == 6 && currentWeapon != 3)
        {
            SwapToSocial();
            handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[3];
            currentWeapon = 3;
        }
        else if(i == 1)
        {
            StartCoroutine(CanSwap());
            SwapToStaff();
            handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[0];
            currentWeapon = 0;
        }
        else if(i == 2)
        {
            StartCoroutine(CanSwap());
            SwapToBook();
            handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[1];
            currentWeapon = 1;
        }
        else if (i == 3)
        {
            StartCoroutine(CanSwap());
            SwapToCrystal();
            handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[2];
            currentWeapon = 2;
        }
        else if (i == 4)
        {
            StartCoroutine(CanSwap());
            currentWeapon += 1;
            if(currentWeapon > 2)
            {
                SwapToStaff();
                handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[0];
                currentWeapon = 0;
            }
            else
            {
                if(currentWeapon == 1)
                {
                    SwapToBook();
                }
                else
                {
                    SwapToCrystal();
                }
                handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[currentWeapon];
            }
        }
        else if (i == 5)
        {
            StartCoroutine(CanSwap());
            currentWeapon -= 1;
            if (currentWeapon < 0)
            {
                SwapToCrystal();
                handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[2];
                currentWeapon = 2;
            }
            else
            {
                if(currentWeapon == 0)
                {
                    SwapToStaff();
                }
                else
                {
                    SwapToBook();
                }
                handForTexture.GetComponent<SpriteRenderer>().sprite = weaponTextures[currentWeapon];
            }
        }
    }


    IEnumerator CanSwap()
    {
        // Timer for swapping weapons.
        canSwap = false;
        for(int i = 0; i < 11; i++)
        {
            swapBar.GetComponent<SpriteRenderer>().sprite = swapbarSprites[i];
            yield return new WaitForSeconds(0.2f);
        }
        canSwap = true;
    }

    IEnumerator CanBurst()
    {
        // Timer for crystal area burst.
        canBurst = false;
        for(int i = 0; i < 10; i++)
        {
            aura.GetComponent<Light2D>().intensity -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 10; i++)
        {
            aura.GetComponent<Light2D>().intensity += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        if (currentWeapon == 2)
        {
            canBurst = true;
        }
    }

    private void Burst()
    {
        if (!canBurst) return;

        StartCoroutine(CanBurst());
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(aura.transform.position.x, aura.transform.position.y), 1f);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hitCollider.GetComponent<EnemyController>().changeHealth(-1);
            }
        }
    }

    public void SwapToStaff()
    {
        canBurst = false;
        aura.SetActive(false);
        projectilePool = projectilePool1;
        timeBetweenShots = 0.5f;
        handForTexture.GetComponent<Light2D>().enabled = false;
    }

    public void SwapToBook()
    {
        canBurst = false;
        aura.SetActive(false);
        projectilePool = projectilePool2;
        timeBetweenShots = 2f;
        handForTexture.GetComponent<Light2D>().enabled = true;
    }

    public void SwapToCrystal()
    {
        canBurst = true;
        aura.SetActive(true);
        handForTexture.GetComponent<Light2D>().enabled = false;
    }

    public void SwapToSocial()
    {
        canBurst = false;
        aura.SetActive(false);
        projectilePool = projectilePool3;
        timeBetweenShots = 0.5f;
        handForTexture.GetComponent<Light2D>().enabled = false;
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
