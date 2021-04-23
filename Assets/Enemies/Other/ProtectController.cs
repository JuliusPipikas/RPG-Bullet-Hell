using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectController : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    private int health;

    [SerializeField]
    GameObject healthBar;

    [SerializeField]
    private List<Sprite> healthbarSprites;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject spawnPoof;

    private void Start()
    {
        health = maxHealth;
        changeHealth(0);

        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
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
            
            Destroy(gameObject, 0.2f);
        }
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
