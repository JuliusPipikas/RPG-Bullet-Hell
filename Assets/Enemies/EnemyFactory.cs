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

    private Actions controls;

    private void Awake()
    {
        controls = new Actions();
        controls.Player.SpawnGobbo.performed += _ => SpawnRandomGobbo();
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
    public void SpawnRandomGobbo()
    {
        float x = Random.Range(-368, 368) / 100f;
        float y = Random.Range(-368, 368) / 100f;
        SpawnEnemy("Goblin", x, y);
    }

    public void SpawnEnemy(string name, float x, float y)
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].name == name)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject enemy = Instantiate(enemies[i], pos, Quaternion.identity);
                EnemyController enemy_child = enemy.transform.Find(name).gameObject.GetComponent<EnemyController>();
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
                enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().setProjectilePool(projectilePool);
                enemy.transform.Find(name).gameObject.GetComponent<EnemyController>().spawnPoof = spawnPoof;
                enemy.transform.parent = transform;
                break;
            }
        }
    }
}
