using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField]
    private GameObject poolObject;

    [SerializeField]
    int initialObjectNumber;

    private List<GameObject> pool;
    // Start is called before the first frame update
    void Start()
    {
        pool = new List<GameObject>();
        for(int i = 0; i < initialObjectNumber; ++i)
        {
            GameObject newObject = Instantiate(poolObject);
            newObject.SetActive(false);
            pool.Add(newObject);
        }
    }

    public GameObject GetFirstAvailableItem()
    {
        foreach(GameObject item in pool)
        {
            if (item.activeInHierarchy) continue;
            else return item;
        }
        //If we get here then the pool is already full, create a new one and add it to the pool.
        GameObject returnObject = Instantiate(poolObject);
        pool.Add(returnObject);
        return returnObject;
    }
}
