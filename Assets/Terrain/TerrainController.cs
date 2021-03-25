using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    GameObject[] stones;
    [SerializeField]
    GameObject[] plants;
    [SerializeField]
    GameObject[] pillars;

    [SerializeField]
    GameObject spawnPoof;

    [SerializeField]
    Material TransparentMaterial;
    [SerializeField]
    Material DefaultMaterial;

    private Actions controls;

    private void Awake()
    {
        controls = new Actions();
        controls.Player.SpawnTerrain.performed += _ => SpawnRandomTerrain();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // For testing
    public void SpawnRandomTerrain()
    {
        float x = Random.Range(-368, 368) / 100f;
        float y = Random.Range(-368, 368) / 100f;
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            SpawnTerrain("Pillar", x, y);
        }
        else if(rand == 1)
        {
            SpawnTerrain("Stone", x, y);
        }
        else
        {
            SpawnTerrain("Plant", x, y);
        }
    }

    public void SpawnTerrain(string name, float x, float y)
    {
        if(name == "Stone")
        {
            int index = Random.Range(0, stones.Length);
            Vector3 pos = new Vector3(x, y, 0);
            GameObject stone = Instantiate(stones[index], pos, Quaternion.identity);
            TerrainInteractions TI = stone.GetComponent<TerrainInteractions>();
            GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);

            Destroy(spawn, 0.3f);

            TI.setTransparentMaterial(TransparentMaterial);
            TI.setDefaultMaterial(DefaultMaterial);
            stone.GetComponent<SpriteRenderer>().sortingOrder = (int)(y * -1000);
            stone.transform.parent = transform;
        }
        if (name == "Plant")
        {
            int index = Random.Range(0, plants.Length);
            Vector3 pos = new Vector3(x, y, 0);
            GameObject plant = Instantiate(plants[index], pos, Quaternion.identity);
            TerrainInteractions TI = plant.GetComponent<TerrainInteractions>();
            GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);

            Destroy(spawn, 0.3f);

            TI.setTransparentMaterial(TransparentMaterial);
            TI.setDefaultMaterial(DefaultMaterial);
            plant.GetComponent<SpriteRenderer>().sortingOrder = (int)(y * -1000);
            plant.transform.parent = transform;
        }
        if (name == "Pillar")
        {
            int index = Random.Range(0, pillars.Length);
            Vector3 pos = new Vector3(x, y, 0);
            GameObject pillar = Instantiate(pillars[index], pos, Quaternion.identity);
            TerrainInteractions TI = pillar.GetComponent<TerrainInteractions>();
            GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);

            Destroy(spawn, 0.3f);

            TI.setTransparentMaterial(TransparentMaterial);
            TI.setDefaultMaterial(DefaultMaterial);
            pillar.GetComponent<SpriteRenderer>().sortingOrder = (int)(y * -1000);
            pillar.transform.parent = transform;
        }
    }
}
