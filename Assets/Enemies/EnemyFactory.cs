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
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].name == name)
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
                GameObject spawn =  Instantiate(spawnPoof, pos, Quaternion.identity);
                
                Destroy(spawn, 0.3f);

                Pooler projectilePool = null;
                for(int u = 0; u < projectiles.Length; u++)
                {
                    if(projectiles[u].name == name + "Projectiles")
                    {
                        projectilePool = projectiles[u];
                        break;
                    }
                }
                if (name != "GoblinStack")
                {
                    enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().setProjectilePool(projectilePool);
                    enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().spawnPoof = spawnPoof;
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
}
