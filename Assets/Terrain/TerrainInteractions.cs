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

    int collission_count = 0;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collission_count++;
            gameObject.GetComponent<SpriteRenderer>().material = transparentMaterial;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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
