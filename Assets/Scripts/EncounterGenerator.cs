using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private bool raiderSpawnTest = false;
    [SerializeField]
    private bool wizardSpawnTest = false;
    [SerializeField]
    private bool chestSpawnTest = false;
    [SerializeField]
    private bool princessSpawnTest = false;

    [SerializeField]
    private List<AudioClip> audioClips;

    [SerializeField]
    private Text encounterText;

    private float maxRadius = 3.5f;

    private GameObject Shovable;


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
            StartCoroutine(spawnGoblinAmbush(intensityTest));

            goblinSpawnTest = false;
        }
        if (skeletonSpawnTest == true)
        {
            StartCoroutine(spawnSkeletalRuins(intensityTest));

            skeletonSpawnTest = false;
        }
        if (raiderSpawnTest == true)
        {
            StartCoroutine(spawnRaiderAttack(intensityTest));

            raiderSpawnTest = false;
        }
        if (wizardSpawnTest == true)
        {
            StartCoroutine(spawnWizardLair(intensityTest));

            wizardSpawnTest = false;
        }
        if (chestSpawnTest == true)
        {
            StartCoroutine(spawnChestProtect(intensityTest));

            chestSpawnTest = false;
        }
        if (princessSpawnTest == true)
        {
            StartCoroutine(spawnPrincessProtect(intensityTest));

            princessSpawnTest = false;
        }
    }

    IEnumerator spawnGoblinAmbush(int intensity)
    {
        if (intensity < 4)
        {
            encounterText.text = "Goblin Ambush!";
        }
        else
        {
            encounterText.text = "Goblin Stack!";
        }
        encounterText.gameObject.SetActive(true);
        StartCoroutine(turnOffAudio());
        if (intensity < 4)
        {
            for (int i = 0; i < Mathf.FloorToInt(3.5f * intensity); i++)
            {
                PlaceTerrainInFreeSpot("Plant", 1f, maxRadius, Vector2.zero);
                PlaceTerrainInFreeSpot("Stone", 1f, maxRadius, Vector2.zero);
            }
        }
        else
        {
            for (int i = 0; i < Mathf.FloorToInt(3.5f * 2); i++)
            {
                PlaceTerrainInFreeSpot("Plant", 1f, maxRadius, Vector2.zero);
                PlaceTerrainInFreeSpot("Stone", 1f, maxRadius, Vector2.zero);
            }
        }

        yield return new WaitForSeconds(2.5f);
        if (intensity < 4)
        {
            for (int i = 0; i < 2 * intensity; i++)
            {
                PlaceEnemyInFreeSpot("Goblin", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
                PlaceEnemyInFreeSpot("GoblinWithSword", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            }
        }
        else
        {
            PlaceEnemyInFreeSpot("GoblinStack", 0.3f, 0.5f, maxRadius - 2, Vector2.zero, 0, false, null);
        }
    }

    IEnumerator spawnRaiderAttack(int intensity)
    {
        if (intensity > 3)
        {
            intensity = 3;
        }

        encounterText.text = "Raider Attack!";
        encounterText.gameObject.SetActive(true);

        StartCoroutine(turnOffAudio());

        for (int i = 0; i < Mathf.FloorToInt(3.5f * intensity); i++)
        {
            PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
            PlaceTerrainInFreeSpot("Stone", 1f, maxRadius, Vector2.zero);
        }

        yield return new WaitForSeconds(2.5f);

        int t = 1;
        if (intensity == 0) t = 0;

        for (int i = 0; i < 1*t + intensity; i++)
        {
            PlaceEnemyInFreeSpot("Bandit", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
        }

        if(intensity == 2)
        {
            PlaceEnemyInFreeSpot("BanditLeader", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            if (intensity == 3)
            {
                PlaceEnemyInFreeSpot("BanditLeader", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            }
        }

    }

    IEnumerator spawnWizardLair(int intensity)
    {
        if (intensity > 3)
        {
            intensity = 3;
        }

        encounterText.text = "Wizard Lair!";
        encounterText.gameObject.SetActive(true);

        StartCoroutine(turnOffAudio());

        for (int i = 0; i < Mathf.FloorToInt(3.5f * intensity); i++)
        {
            PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
            PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
        }

        yield return new WaitForSeconds(2.5f);

        if(intensity != 3)
        {
            for (int i = 0; i < 2 * intensity; i++)
            {
                PlaceEnemyInFreeSpot("Slime", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            }
        }
        else
        {
            for (int i = 0; i < intensity; i++)
            {
                PlaceEnemyInFreeSpot("Slime", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            }
            PlaceEnemyInFreeSpot("Wizard", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
        }

        

    }

    IEnumerator turnOffAudio()
    {
        yield return new WaitForSeconds(2f);
        encounterText.gameObject.SetActive(false);
    }

    IEnumerator spawnSkeletalRuins(int intensity)
    {
        if (intensity < 4)
        {
            encounterText.text = "Skeletal Ruins!";
        }
        else
        {
            encounterText.text = "Necromancer!";
        }
        encounterText.gameObject.SetActive(true);
        StartCoroutine(turnOffAudio());

        if (intensity < 4)
        {
            for (int i = 0; i < 7 * intensity; i++)
            {
                PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
            }
        }
        else
        {
            for (int i = 0; i < 7 * 2; i++)
            {
                PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
            }
        }

        yield return new WaitForSeconds(2.5f);

        if (intensity < 4)
        {
            for (int i = 0; i < 2 * intensity; i++)
            {
                PlaceEnemyInFreeSpot("Skeleton", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
                PlaceEnemyInFreeSpot("SkeletonWithSword", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            }
        }
        else
        {
            PlaceEnemyInFreeSpot("Necromancer", 0.3f, 0.5f, maxRadius-2, Vector2.zero, 0, false, null);
        }
    }

    IEnumerator spawnChestProtect(int intensity)
    {
        if (intensity > 3)
        {
            intensity = 3;
        }

        encounterText.text = "Hand over ye goods!";

        encounterText.gameObject.SetActive(true);

        StartCoroutine(turnOffAudio());

        PlaceShovableInFreeSpot("Chest", 0.3f, 0f, 1.5f, Vector2.zero);

        for (int i = 0; i < Mathf.FloorToInt(3.5f * intensity); i++)
        {
            PlaceTerrainInFreeSpot("Plant", 1f, maxRadius, Vector2.zero);
            PlaceTerrainInFreeSpot("Stone", 1f, maxRadius, Vector2.zero);
        }

        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < intensity; i++)
        {
            PlaceEnemyInFreeSpot("Bandit", 0.3f, 0.5f, maxRadius, Vector2.zero, 30, true, Shovable);
        }

    }

    IEnumerator spawnPrincessProtect(int intensity)
    {
        if (intensity > 3)
        {
            intensity = 3;
        }

        encounterText.text = "Rescue her!";

        encounterText.gameObject.SetActive(true);

        StartCoroutine(turnOffAudio());

        PlaceShovableInFreeSpot("Princess", 0.3f, 0f, 1.5f, Vector2.zero);

        for (int i = 0; i < Mathf.FloorToInt(3.5f * intensity); i++)
        {
            PlaceTerrainInFreeSpot("Plant", 1f, maxRadius, Vector2.zero);
            PlaceTerrainInFreeSpot("Pillar", 1f, maxRadius, Vector2.zero);
        }

        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < intensity; i++)
        {
            PlaceEnemyInFreeSpot("Goblin", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
            PlaceEnemyInFreeSpot("Goblin", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, true, Shovable);
            PlaceEnemyInFreeSpot("GoblinWithSword", 0.3f, 0.5f, maxRadius, Vector2.zero, 0, false, null);
        }

    }

    public void PlaceEnemyInFreeSpot(string name, float rangeDistance, float spawnRadiusMin, float spawnRadius, Vector2 offset, int customHP, bool towardsProtection, GameObject protection)
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
                if (objs != null)
                {
                    Transform tr = objs.transform;
                    float distance = Vector2.Distance(spawnPos, tr.position);

                    if (ind == 0)
                    {
                        if (distance < rangeDistance + 1f)
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
                }
                ind++;
            }

            if (foundFreeSpot)
            {
                objects.Add(ef.SpawnEnemy(name, spawnPos.x, spawnPos.y, customHP, towardsProtection, protection));
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

    public void PlaceShovableInFreeSpot(string name, float rangeDistance, float spawnRadiusMin, float spawnRadius, Vector2 offset)
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
            float radius = Random.Range(spawnRadiusMin, spawnRadius);
            Vector2 spawnPos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;

            int ind = 0;

            foreach (GameObject objs in objects)
            {
                if (objs != null)
                {
                    Transform tr = objs.transform;
                    float distance = Vector2.Distance(spawnPos, tr.position);

                    if (ind == 0)
                    {
                        if (distance < rangeDistance + 1f)
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
                }
                ind++;
            }

            if (foundFreeSpot)
            {
                Shovable = ef.SpawnShovable(name, spawnPos.x, spawnPos.y);
                objects.Add(Shovable);
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
    }
}
