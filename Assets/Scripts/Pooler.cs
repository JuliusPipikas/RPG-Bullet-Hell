using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int poolSize = 10;

    [SerializeField]
    private bool expandable = false;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }

    public void setExpandable()
    {
        expandable = true;
    }

    public void simulateAwake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }

    public List<GameObject> getActiveList()
    {
        return usedList;
    }

    public List<GameObject> getUsedList()
    {
        return freeList;
    }

    private void GenerateNewObject()
    {
        Vector3 dir = Vector3.left;
        GameObject prj;
        if (prefab)
        {
            prj = Instantiate(prefab);
        }
        else
        {
            prj = new GameObject();
        }
        prj.transform.parent = transform;
        prj.SetActive(false);
        freeList.Add(prj);
    }

    public GameObject GetObject(){
        int totalFree = freeList.Count;
        if (freeList.Count == 0 && !expandable) return null;
        else if (freeList.Count == 0 && expandable)
        {
            GenerateNewObject();
            totalFree++;
        }

        GameObject prj = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(prj);
        return prj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }
}
