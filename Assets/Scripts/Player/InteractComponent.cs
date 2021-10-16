using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] private float radius = .6F;
    
    private List<GameObject> _objectList = new List<GameObject>();

    public void Interact()
    {
        var obj = GetClosestObject();
        if (obj == null) return;
        obj.Interact();
    }

    private void CreateObjectList()
    {
        // get all colliders within box area 
        var colliders = Physics.OverlapSphere(transform.position, radius);
        // loop through and creat a list of all the interactable objects
        foreach (var c in colliders)
        {
            if ( c.GetComponent<IInteractable>() == null ) continue;
            _objectList.Add(c.gameObject);
        }
    }

    private IInteractable GetClosestObject()
    {
        CreateObjectList();

        var pos = transform.position;
        GameObject closestObj = null;
        var dist = 10f;
        
        foreach (var obj in _objectList)
        {
            var temp = Vector3.Distance(pos, obj.transform.position);
            if (temp < dist) closestObj = obj;
        }
        
        return closestObj == null ? null : closestObj.GetComponent<IInteractable>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
