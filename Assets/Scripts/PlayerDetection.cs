using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField]
    private float angleOfView = 25f;
    [SerializeField]
    private float maxViewDistance = 10f;
    [SerializeField]
    private int numberOfRaycasts = 10;

    // Update is called once per frame
    void Update()
    {
        if(CheckObjectInSight())
        {
            SeesPlayer();
        }
        else
        {
            DoesNotSeePlayer();
        }
    }

    bool CheckObjectInSight()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        for (int i=0; i <= numberOfRaycasts; ++i)
        {
            float t = (float)i / (float)numberOfRaycasts;
            float rayAngle = Mathf.Lerp(angleOfView, -angleOfView, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector2 direction = RotateVectorByDegrees(transform.up, rayAngle);

            Physics2D.Raycast(transform.position, direction, new ContactFilter2D(), hits, maxViewDistance);
            if (hits.Count > 0 && IsPlayer(hits[0].collider.gameObject))
            {
                return true;
            }
        }
        return false;
    }

    Vector2 RotateVectorByDegrees(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;

        float x2 = (Mathf.Cos(radians) * vector.x) - (Mathf.Sin(radians) * vector.y);
        float y2 = (Mathf.Sin(radians) * vector.x) + (Mathf.Cos(radians) * vector.y);
        return new Vector2(x2, y2);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i <= numberOfRaycasts; ++i)
        {
            float t = (float)i / (float)numberOfRaycasts;
            float rayAngle = Mathf.Lerp(angleOfView, -angleOfView, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector2 direction = RotateVectorByDegrees(transform.up, rayAngle);

            //Debug.DrawRay(transform.position, direction, Color.red);

            Gizmos.DrawRay(transform.position, transform.up);
            Gizmos.DrawLine(transform.position, ((Vector2)transform.position + direction * maxViewDistance));
        }
    }

    private bool IsPlayer(GameObject other)
    {
        return other.name == "Test";
    }

    private void SeesPlayer()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    private void DoesNotSeePlayer()
    {
        GetComponentInChildren<Renderer>().material.color = Color.green;
    }
}
