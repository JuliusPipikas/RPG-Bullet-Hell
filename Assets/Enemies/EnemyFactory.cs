using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{

    [SerializeField]
    GameObject[] enemies;

    [SerializeField]
    Pooler[] projectiles;

    [SerializeField]
    GameObject spawnPoof;

    [SerializeField]
    GameObject[] shovables;

    [SerializeField]
    GameObject[] villagers;

    public GameObject SpawnEnemy(string name, float x, float y, int customHP, bool towardsProtection, GameObject protection)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].name == name)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject enemy = Instantiate(enemies[i], pos, Quaternion.identity);
                EnemyController enemy_child = enemy.transform.Find(name).gameObject.GetComponent<EnemyController>();
                if (customHP != 0)
                {
                    enemy_child.setMaxHealth(customHP);
                }

                if (towardsProtection)
                {
                    enemy_child.setMoveTowardsProtection(towardsProtection);
                    enemy_child.setProtection(protection);
                }

                if (spawnPoof)
                {
                    GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);
                    Destroy(spawn, 0.3f);
                }

                Pooler projectilePool = null;
                for (int u = 0; u < projectiles.Length; u++)
                {
                    if (projectiles[u].name == name + "Projectiles")
                    {
                        projectilePool = projectiles[u];
                        break;
                    }
                }
                if (name != "GoblinStack")
                {
                    enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().setProjectilePool(projectilePool);
                    if (spawnPoof)
                    {
                        enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().spawnPoof = spawnPoof;
                    }
                }
                else
                {
                    enemy.transform.Find(name).gameObject.GetComponent<StackController>().setProjectilePool(projectilePool);
                    enemy.transform.Find(name).gameObject.GetComponent<StackController>().spawnPoof = spawnPoof;
                }
                enemy.transform.parent = transform;
                return enemy;
            }
        }
        return null;
    }

    public void MockVillagers()
    {
        villagers = new GameObject[1];
        villagers[0] = new GameObject("TestVillager");
        villagers[0].AddComponent<VillagerController>();
    }

    public void MockEnemies()
    {
        GameObject tempChild = new GameObject();
        tempChild.AddComponent<EnemyController>();

        enemies = new GameObject[4];
        enemies[0] = new GameObject("TestGoblin");
        tempChild = Instantiate(tempChild, Vector3.zero, Quaternion.identity, enemies[0].transform);
        tempChild.name = "TestGoblin";
        enemies[1] = new GameObject("TestSkeleton");
        tempChild = Instantiate(tempChild, Vector3.zero, Quaternion.identity, enemies[1].transform);
        tempChild.name = "TestSkeleton";
        enemies[2] = new GameObject("TestRaider");
        tempChild = Instantiate(tempChild, Vector3.zero, Quaternion.identity, enemies[2].transform);
        tempChild.name = "TestRaider";
        enemies[3] = new GameObject("TestWizard");
        tempChild = Instantiate(tempChild, Vector3.zero, Quaternion.identity, enemies[3].transform);
        tempChild.name = "TestWizard";

        GameObject temp;
        
        projectiles = new Pooler[4];
        temp = new GameObject("TestGoblinProjectiles");
        temp.AddComponent<Pooler>();
        projectiles[0] = temp.GetComponent<Pooler>();
        temp = new GameObject("TestSkeletonProjectiles");
        temp.AddComponent<Pooler>();
        projectiles[1] = temp.GetComponent<Pooler>();
        temp = new GameObject("TestRaiderProjectiles");
        temp.AddComponent<Pooler>();
        projectiles[2] = temp.GetComponent<Pooler>();
        temp = new GameObject("TestWizardProjectiles");
        temp.AddComponent<Pooler>();
        projectiles[3] = temp.GetComponent<Pooler>();
    }

    public GameObject SpawnShovable(string name, float x, float y)
    {
        for (int i = 0; i < shovables.Length; i++)
        {
            if (shovables[i].name == name)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject shovable = Instantiate(shovables[i], pos, Quaternion.identity);
                GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);

                Destroy(spawn, 0.3f);

                shovable.GetComponent<ProtectController>().spawnPoof = spawnPoof;
                return shovable;
            }
        }
        return null;
    }

    public GameObject SpawnVillager(string name, float x, float y, int intensity)
    {
        for (int i = 0; i < villagers.Length; i++)
        {
            if (villagers[i].name == name)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject villager = Instantiate(villagers[i], pos, Quaternion.identity);
                if (spawnPoof)
                {
                    GameObject spawn = Instantiate(spawnPoof, pos, Quaternion.identity);
                    Destroy(spawn, 0.3f);
                    villager.GetComponent<VillagerController>().spawnPoof = spawnPoof;
                }
                villager.GetComponent<VillagerController>().changeMoveIntensity(intensity);
                return villager;
            }
        }
        return null;
    }
}
