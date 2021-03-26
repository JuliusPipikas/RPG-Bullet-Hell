using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterGenerator : MonoBehaviour
{

    private List<GameObject> objects;
    [SerializeField]
    private EnemyFactory ef;
    [SerializeField]
    private TerrainController tc;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject SpawnPoof;

    [SerializeField]
    private int intensityTest = 1;
    [SerializeField]
    private bool despawnTest = false;
    [SerializeField]
    private bool goblinSpawnTest = false;
    [SerializeField]
    private bool skeletonSpawnTest = false;

    private float maxRadius = 3.5f;


    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
        objects.Add(Player);
    }

    // Update is called once per frame
    void Update()
    {
        if(despawnTest == true)
        {
            StartCoroutine(Despawn());

            despawnTest = false;
        }
        if (goblinSpawnTest == true)
        {
            spawnGoblinAmbush(intensityTest);

            goblinSpawnTest = false;
        }
        if (skeletonSpawnTest == true)
        {
            spawnSkeletalRuins(intensityTest);

            skeletonSpawnTest = false;
        }
    }

    public void spawnGoblinAmbush(int intensity)
    {
        for(int i = 0; i < Mathf.FloorToInt(3.5f*intensity); i++)
        {
            PlaceTerrainInFreeSpot("Plant", 1f, maxRadius, Vector2.zero);
            PlaceTerrainInFreeSpot("Stone", 1f, maxRadius, Vector2.zero);
        }

        for(int i = 0; i < 2*intensity; i++)
        {
            PlaceEnemyInFreeSpot("Goblin", 0.3f, 0.5f, maxRadius, Vector2.zero);
            PlaceEnemyInFreeSpot("GoblinWithSword", 0.3f, 0.5f, maxRadius, Vector2.zero);
        }
    }

    public void spawnSkeletalRuins(int intensity)
    {
        for (int i = 0; i < 7 * intensity; i++)
        {
            PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
        }

        for (int i = 0; i < 2 * intensity; i++)
        {
            PlaceEnemyInFreeSpot("Skeleton", 0.3f, 0.5f, maxRadius, Vector2.zero);
            PlaceEnemyInFreeSpot("GoblinWithSword", 0.3f, 0.5f, maxRadius, Vector2.zero);
        }
    }

    public void PlaceEnemyInFreeSpot(string name, float rangeDistance, float spawnRadiusMin, float spawnRadius, Vector2 offset)
    {
        bool foundFreeSpot = false;

        while (foundFreeSpot == false)
        {
            foundFreeSpot = true;

            float radians = Random.Range(0, 360) * Mathf.Deg2Rad;
            if(spawnRadius > maxRadius)
            {
                spawnRadius = maxRadius;
            }
            float radius = Random.Range(spawnRadiusMin, spawnRadius);
            Vector2 spawnPos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;

            int ind = 0;

            foreach(GameObject objs in objects)
            {

                Transform tr = objs.transform;
                float distance = Vector2.Distance(spawnPos, tr.position);

                if (ind == 0)
                {
                    if (distance < rangeDistance+1f)
                    {
                        foundFreeSpot = false;
                    }
                }
                else
                {
                    if (distance < rangeDistance)
                    {
                        foundFreeSpot = false;
                    }
                }

                ind++;
            }

            if (foundFreeSpot)
            {
                objects.Add(ef.SpawnEnemy(name, spawnPos.x, spawnPos.y));
            }

        }
    }

    public void PlaceTerrainInFreeSpot(string name, float rangeDistance, float spawnRadius, Vector2 offset)
    {
        bool foundFreeSpot = false;

        while (foundFreeSpot == false)
        {
            foundFreeSpot = true;

            float radians = Random.Range(0, 360) * Mathf.Deg2Rad;
            if (spawnRadius > maxRadius)
            {
                spawnRadius = maxRadius;
            }
            float radius = Random.Range(0f, spawnRadius);
            Vector2 spawnPos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;


            foreach (GameObject objs in objects)
            {
                Transform tr = objs.transform;
                float distance = Vector2.Distance(spawnPos, tr.position);
                if (distance < rangeDistance)
                {
                    foundFreeSpot = false;
                }

            }
            

            if (foundFreeSpot)
            {
                objects.Add(tc.SpawnTerrain(name, spawnPos.x, spawnPos.y));
            }

        }
    }

    public IEnumerator Despawn()
    {
        for(int i = 1; i < objects.Count; i++)
        {
            if (objects[i] != null)
            {
                GameObject spawn = Instantiate(SpawnPoof, objects[i].transform.position, Quaternion.identity);
                Destroy(spawn, 0.3f);
                Destroy(objects[i]);
            }
        }

        yield return new WaitForSeconds(0.31f);

        objects = new List<GameObject>();
        objects.Add(Player);
        Debug.Log(objects.Count + " objects remain");
    }
}
