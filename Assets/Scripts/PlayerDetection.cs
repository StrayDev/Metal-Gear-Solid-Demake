using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] 
    private UnityEvent onPlayerDetected = default;


    [SerializeField]
    private float angleOfView = 25f;
    [SerializeField]
    private float maxViewDistance = 10f;
    [SerializeField]
    private int numberOfRaycasts = 10;
    [SerializeField]
    private float shootCooldown = 3f;

    private float cooldownRemaining = 0f;

    private bool seenPlayer = false;

    // Update is called once per frame
    void Update()
    {
        if (cooldownRemaining > 0) cooldownRemaining -= Time.deltaTime;

        Transform playerLocation = CheckObjectInSight();
        if (playerLocation != null)
        {
            SeesPlayer(playerLocation);
        }
        else
        {
            DoesNotSeePlayer();
        }
    }

    Transform CheckObjectInSight()
    {
        RaycastHit hit = new RaycastHit();
        for (int i=0; i <= numberOfRaycasts; ++i)
        {
            float t = (float)i / (float)numberOfRaycasts;
            float rayAngle = Mathf.Lerp(angleOfView, -angleOfView, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector3 direction = RotateVectorByDegrees(transform.up, rayAngle);

            Physics.Raycast(transform.position, direction, out hit, maxViewDistance, ~(1 << 2));
            if (hit.collider != null && IsPlayer(hit.collider.gameObject))
            {
                return hit.collider.transform;
            }
        }
        return null;
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
        return other != null && other.GetComponentInParent<PlayerController>();
    }

    private void SeesPlayer(Transform player)
    {
        // Call player detected events if this is the first time the player is detected since the guard lost sight
        if(!seenPlayer)
        {
            onPlayerDetected?.Invoke();
            seenPlayer = true;
        }

        GetComponentInChildren<Renderer>().material.color = Color.red;
        if (GetComponent<FireBullet>())
        {
            if (cooldownRemaining <= 0)
            {
                GetComponent<FireBullet>().Fire(transform.position, (player.position - transform.position).normalized,transform.position.z);
                cooldownRemaining = shootCooldown;
            }
        }
    }

    private void DoesNotSeePlayer()
    {
        GetComponentInChildren<Renderer>().material.color = Color.green;

        seenPlayer = false;
    }
}
