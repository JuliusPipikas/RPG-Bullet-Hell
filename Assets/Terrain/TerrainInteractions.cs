using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInteractions : MonoBehaviour
{
    [SerializeField]
    private Material transparentMaterial;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private GameObject player;
    public GameObject shadow;
    public GameObject instantiatedShadow;

    int collission_count = 0;

    private void Awake()
    {
        player = GameObject.Find("Player");

        float offset = 0f;
        if (gameObject.name.Contains("Pillar"))
        {
            offset = 0.06f;
        }
        Vector3 shadowPos = new Vector3(0, -gameObject.GetComponent<SpriteRenderer>().bounds.size.y+offset, 0) + gameObject.transform.localPosition;
        shadow.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        instantiatedShadow = Instantiate(shadow, shadowPos, Quaternion.identity, gameObject.transform);
        instantiatedShadow.transform.rotation = new Quaternion(0, 0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("ShovableObject"))
        {
            collission_count++;
            gameObject.GetComponent<SpriteRenderer>().material = transparentMaterial;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("ShovableObject"))
        {
            collission_count--;
            if (collission_count == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            }
        }
    }

    public void setTransparentMaterial(Material mat)
    {
        transparentMaterial = mat;
    }

    public void setDefaultMaterial(Material mat)
    {
        defaultMaterial = mat;
    }
}
