using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedExclamationUI : MonoBehaviour
{
    [SerializeField] private float distanceAboveTarget = 1.0f;
    [SerializeField] private float sceneFrontDepth = -2.0f;
    [SerializeField] private float lifetimeSeconds = 2.0f;

    private GameObject owningObject = null;
    private float spawnTime = 0.0f;

    public void SetOwner(GameObject owner)
    {
        owningObject = owner;
    }

    private void UpdatePosition()
    {
        // Move the ui to the owning object's position
        this.transform.position = owningObject.transform.position;

        // Bring the ui to the front of the scene
        this.transform.position = new Vector3(transform.position.x, transform.position.y + distanceAboveTarget, sceneFrontDepth);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        // If the ui has expired
        if((Time.time - spawnTime) > lifetimeSeconds)
        {
            // Destroy the ui
            Destroy(this.gameObject);
        }
    }
}
