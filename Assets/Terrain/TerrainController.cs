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

    public GameObject SpawnTerrain(string name, float x, float y)
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
            return stone;
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
            return plant;
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
            return pillar;
        }
        return null;
    }
}
