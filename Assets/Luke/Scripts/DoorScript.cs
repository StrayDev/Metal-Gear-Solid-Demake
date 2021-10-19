using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    //Access level of doors. 0 means no keycards needed.
    [SerializeField][Range(0,5)] private int access_level = 0;
    
    //game object marking the location of where the door will lead. 
    [SerializeField] private GameObject location_marker; 

    //getter for the access level of a door.
    public int getAccessLevel() => access_level;

    //The location markers transform.
    public Vector3 travel_location {get { return location_marker.transform.position; } private set { location_marker.transform.position = value; }}
}

