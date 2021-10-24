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
    private GameObject detectedWorldSpaceUI = null;
    [SerializeField]
    private SoundController soundController = null;
    [SerializeField]
    private AudioClip detectedAudioClip = null;

    [SerializeField]
    private float peripheralAngleOfView = 25f;
    [SerializeField]
    private float directAngleOfView = 5f;
    [SerializeField]
    private float normalViewDistance = 10f;

    [SerializeField]
    private int peripheralNumberOfRaycasts = 10;
    [SerializeField]
    private int directNumberOfRaycasts = 20;

    private float alertViewDistance;
    private float maxViewDistance;

    SimpleGuardBrain brain;
    private void Start()
    {
        brain = GetComponent<SimpleGuardBrain>();
        maxViewDistance = normalViewDistance;
        alertViewDistance = normalViewDistance * 1.5f;
        soundController = FindObjectOfType<SoundController>();
    }

    private bool seenPlayer = false;

    // Update is called once per frame
    void Update()
    {
        Transform playerLocation = CheckObjectInDirectSight();
        if (playerLocation != null)
        {
            SeesPlayer();
        }
        else
        {
            playerLocation = CheckObjectInPeripheralSight();
            if(playerLocation != null)
            {
                PlayerSlightlyVisible();
            }
            else
            {
                DoesNotSeePlayer();
            }
        }
    }

    Transform CheckObjectInPeripheralSight()
    {
        return RaycastVisionCone(peripheralAngleOfView, maxViewDistance, peripheralNumberOfRaycasts);
    }

    Transform CheckObjectInDirectSight()
    {
        return RaycastVisionCone(directAngleOfView, maxViewDistance / 2f, directNumberOfRaycasts);
    }

    Transform RaycastVisionCone(float angle, float distance, int raycastNumber)
    {
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i <= raycastNumber; ++i)
        {
            float t = (float)i / (float)raycastNumber;
            float rayAngle = Mathf.Lerp(angle, -angle, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector3 direction = RotateVectorByDegrees(transform.up, rayAngle);

            Physics.Raycast(transform.position, direction, out hit, distance, ~(1 << 2));
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
        Gizmos.color = Color.yellow;
        for (int i = 0; i <= peripheralNumberOfRaycasts; ++i)
        {
            float t = (float)i / (float)peripheralNumberOfRaycasts;
            float rayAngle = Mathf.Lerp(peripheralAngleOfView, -peripheralAngleOfView, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector3 direction = RotateVectorByDegrees(transform.up, rayAngle);

            //Debug.DrawRay(transform.position, direction, Color.red);

            Gizmos.DrawRay(transform.position, transform.up);
            Gizmos.DrawLine(transform.position, (transform.position + direction * maxViewDistance));
        }
        Gizmos.color = Color.red;
        for (int i = 0; i <= directNumberOfRaycasts; ++i)
        {
            float t = (float)i / (float)directNumberOfRaycasts;
            float rayAngle = Mathf.Lerp(directAngleOfView, -directAngleOfView, t);

            //Vector2 dtv = DegreesToVector2(rayAngle);
            Vector3 direction = RotateVectorByDegrees(transform.up, rayAngle);

            //Debug.DrawRay(transform.position, direction, Color.red);

            Gizmos.DrawRay(transform.position, transform.up);
            Gizmos.DrawLine(transform.position, (transform.position + direction * maxViewDistance/2));
        }
    }

    private bool IsPlayer(GameObject other)
    {
        return other != null && other.GetComponentInParent<PlayerController>();
    }

    private void SeesPlayer()
    {
        // Call player detected events if this is the first time the player is detected since the guard lost sight
        if(!seenPlayer)
        {
            // Increment the number of times the player is detected
            GameController.Instance.playerDetectedCount += 1;

            // Spawn detected ui
            var ui = Instantiate(detectedWorldSpaceUI);
            var wsUI = ui.GetComponent<DetectedExclamationUI>();
            if(wsUI)
            {
                wsUI.SetOwner(this.gameObject);
            }

            // Play detected sound
            soundController.PlaySoundClipOneShot(detectedAudioClip);

            // Call bound onPlayerDetected events
            onPlayerDetected?.Invoke();

            // Flag player is currently seen by the guard
            seenPlayer = true;
        }

        maxViewDistance = alertViewDistance;

        GetComponentInChildren<Renderer>().material.color = Color.red;
        brain.SetPlayerVision(SimpleGuardBrain.PlayerVisibility.DIRECT);
    }

    private void DoesNotSeePlayer()
    {
        maxViewDistance = normalViewDistance;

        GetComponentInChildren<Renderer>().material.color = Color.green;
        brain.SetPlayerVision(SimpleGuardBrain.PlayerVisibility.NONE);
        seenPlayer = false;
    }

    private void PlayerSlightlyVisible()
    {
        maxViewDistance = alertViewDistance;
        GetComponentInChildren<Renderer>().material.color = Color.yellow;
        brain.SetPlayerVision(SimpleGuardBrain.PlayerVisibility.PERIPHERAL);
        seenPlayer = false;
    }
}
