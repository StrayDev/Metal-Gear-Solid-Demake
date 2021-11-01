using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SimpleGuardBrain : MonoBehaviour
{
    public enum PlayerVisibility
    {
        NONE = 0,
        DIRECT = 1,
        PERIPHERAL = 2
    }

    public enum GuardState
    {
        PATROL = 0,
        COMBAT = 1,
        SEARCH = 2,
        SPOTTED = 3,
    };

    private enum AlertType
    {
        HESITATE = 0,
        ALERT = 1,
    }

    [SerializeField]
    private UnityEvent onPlayerDetected = default;
    [SerializeField]
    private GameObject detectedWorldSpaceUI = null;
    [SerializeField]
    private GameObject spottedWorldSpaceUI = null;
    [SerializeField]
    private SoundController soundController = null;
    [SerializeField]
    private AudioClip detectedAudioClip = null;
    [SerializeField]
    private AudioClip questionAudioClip = null;

    // Start is called before the first frame update
    private int current_node_position = 0;
    private Transform guard_pos;
    private MoveComponent guard_movement_comp;
    private GuardState currentState;
    private PlayerVisibility currentVisibility;
    private PlayerController player;
    private Rigidbody playerRb;

    private Coroutine patrolRoutine;
    private GameObject alertUI;

    private LevelController levelController;
    private Pathfinding pathFinder;
    private NodeGrid grid;

    private Vector3 lastSeenPlayerPos;
    private Vector2 lastSeenPlayerVelocity;

    [SerializeField]
    private float surprisedTime = 1f;
    private float surprisedWait = 0f;

    [SerializeField]
    private float searchTime = 10f;
    private float searchCooldown = 0f;

    [SerializeField]
    private float dismissTime = 3f;
    private float dismissWait = 0f;

    private bool alerted = false;

    private List<Node> currentPath;

    [SerializeField] private bool enable_debug_messages = false;
    [SerializeField] private List<GuardNode> nodes;

    void Start()
    {
        guard_movement_comp = GetComponent<MoveComponent>();
        guard_pos = GetComponent<Transform>();
        levelController = FindObjectOfType<LevelController>();
        grid = levelController?.GetGrid() ?? FindObjectOfType<NodeGrid>();
        pathFinder = levelController?.GetPathAI();
        soundController = FindObjectOfType<SoundController>();
        patrolRoutine = StartCoroutine("processNodes");

        currentPath = new List<Node>();
    }

    public PlayerVisibility GetVisibility()
    {
        return currentVisibility;
    }

    public void SetPlayerVision(PlayerVisibility visibility)
    {
        currentVisibility = visibility;
    }

    private void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
            playerRb = player.GetComponent<Rigidbody>();
        }

        switch (currentState)
        {
            case GuardState.PATROL:
                {
                    if(currentPath.Count > 0)
                    {
                        if(MoveToNextNode(1f) == false)
                        {
                            ResumePatrol();
                        }
                    }
                    if(currentVisibility > 0)
                    {
                        SpotPlayer();
                    }
                    break;
                }
            case GuardState.COMBAT:
                {

                    // Can we see the player still? If not we need to look for them
                    if(currentVisibility == PlayerVisibility.NONE)
                    {
                        StartSearch();
                        break;
                    }

                    lastSeenPlayerPos = player.transform.position;
                    lastSeenPlayerVelocity = playerRb.velocity;

                    // Are we aiming at the player?
                    Vector3 direction = (player.transform.position - transform.position);
                    bool seePlayer = false;
                    RaycastHit[] hits;
                    if ((hits = Physics.RaycastAll(transform.position, transform.up)).Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            if(hit.collider.GetComponentInParent<PlayerController>())
                            {
                                seePlayer = true;
                                break;
                            }
                        }
                    }

                    if (seePlayer)
                    {
                        // If we are looking directly at the player, shoot at them!
                        FireBullet fireBullet;
                        if ((fireBullet = GetComponent<FireBullet>()) != null)
                        {
                            fireBullet.Fire(transform.position, (player.transform.position - transform.position), transform.position.z);
                        }
                    }
                    // Otherwise we need to turn towards them
                    else
                    {
                        TurnToFacePlayer(5f);
                    }
                    break;
                }
            case GuardState.SEARCH:
                {
                    if((int)currentVisibility > 0)
                    {
                        lastSeenPlayerPos = player.transform.position;
                        lastSeenPlayerVelocity = playerRb.velocity;
                        EngageCombat();
                        break;
                    }
                    if (currentPath?.Count > 0)
                    {
                        if(MoveToNextNode(2f) == false)
                        {
                            guard_movement_comp.Direction(lastSeenPlayerVelocity);
                        }
                    }
                    if ((searchCooldown -= Time.deltaTime) <= 0f)
                    {
                        ReturnToRoute();
                    }
                    break;
                }
            case GuardState.SPOTTED:
            {
                if(currentVisibility > 0)
                {
                    lastSeenPlayerPos = player.transform.position;
                    lastSeenPlayerVelocity = playerRb.velocity;
                    dismissWait = dismissTime;
                    TurnToFacePlayer(1f);
                }
                switch(currentVisibility)
                {
                    case PlayerVisibility.NONE:
                        {
                            guard_movement_comp.Move(Vector2.zero);
                            if ((dismissWait -= Time.deltaTime) <= 0f)
                            {
                                ReturnToRoute();
                            }
                            break;
                        }
                    case PlayerVisibility.PERIPHERAL:
                        {
                            if ((surprisedWait -= Time.deltaTime) <= 0f)
                            {
                                EngageCombat();
                            }
                            break;
                        }
                    case PlayerVisibility.DIRECT:
                        {
                            EngageCombat();
                            break;
                        }
                }
                break;
            }
        }
    }

    public void SpotPlayer()
    {
        if (currentState == GuardState.SPOTTED) return;

        lastSeenPlayerPos = player.transform.position;
        lastSeenPlayerVelocity = playerRb.velocity;
        currentState = GuardState.SPOTTED;
        if (patrolRoutine != null)
        {
            StopCoroutine(patrolRoutine);
            guard_movement_comp.Move(Vector2.zero);
            patrolRoutine = null;
        }
        PlaySound(AlertType.HESITATE);
        surprisedWait = surprisedTime;
    }

    public void EngageCombat()
    {
        if (currentState == GuardState.COMBAT) return;
        
        if(alerted == false)
        {
            alerted = true;
            PlaySound(AlertType.ALERT);
        }

        currentPath.Clear();

        currentState = GuardState.COMBAT;
        guard_movement_comp.Move(Vector2.zero);
    }

    private void PlaySound(AlertType alert)
    {
        if (alertUI != null)
        {
            Destroy(alertUI);
            alertUI = null;
        }
        if (alert == AlertType.HESITATE)
        {
            alertUI = Instantiate(spottedWorldSpaceUI);
            soundController.PlaySoundClipOneShot(questionAudioClip);
        }
        else
        {
            alertUI = Instantiate(detectedWorldSpaceUI);
            soundController.PlaySoundClipOneShot(detectedAudioClip);
            GameController.Instance.playerDetectedCount += 1;
        }

        // Spawn detected ui
        var wsUI = alertUI.GetComponent<DetectedExclamationUI>();
        if (wsUI)
        {
            wsUI.SetOwner(this.gameObject);
        }
    }

    public void ReturnToRoute()
    {
        alerted = false;
        currentState = GuardState.PATROL;
        if (grid == null) grid = levelController.GetGrid();
        Node startNode = grid.GetNode(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
        Vector2 patrolPos = nodes[current_node_position].position.position;
        Node endNode = grid.GetNode(new Vector2Int(Mathf.RoundToInt(patrolPos.x), Mathf.RoundToInt(patrolPos.y)));
        if (pathFinder == null)
        {
            levelController = FindObjectOfType<LevelController>();
            pathFinder = levelController.GetPathAI();
        }
        List<Node> newPath = pathFinder.FindPath(startNode, endNode);
        if (newPath != null) currentPath = newPath;
    }

    public void ResumePatrol()
    {
        patrolRoutine = StartCoroutine(processNodes());
    }

    public void StartSearch()
    {
        currentState = GuardState.SEARCH;
        searchCooldown = searchTime;
        if (grid == null) grid = levelController.GetGrid();
        Node startNode = grid.GetNode(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
        Vector2 playerPos = lastSeenPlayerPos;
        Node endNode = grid.GetNode(new Vector2Int(Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.y)));
        if (pathFinder == null)
        {
            levelController = FindObjectOfType<LevelController>();
            pathFinder = levelController.GetPathAI();
        }
        List<Node> newPath = pathFinder.FindPath(startNode, endNode);
        if (newPath != null) currentPath = newPath;
    }

    private bool MoveToNextNode(float speed)
    {
        Vector3 nextPos = new Vector3(currentPath[0].x, currentPath[0].y, guard_pos.position.z);
        if (Vector3.Distance(nextPos, guard_pos.position) <= 0.8f)
        {
            currentPath.RemoveAt(0);
            if (currentPath.Count > 0)
            {
                nextPos.x = currentPath[0].x;
                nextPos.y = currentPath[0].y;
            }
            else
            {
                guard_movement_comp.Move(Vector2.zero);
                return false;
            }
        }
        guard_movement_comp.Move(nextPos - transform.position, speed);
        guard_movement_comp.Direction((nextPos - transform.position).normalized);
        return true;
    }

    private void TurnToFacePlayer(float turnSpeed)
    {
        Vector2 targetDirection = (Vector2)player.transform.position - (Vector2)guard_pos.position;
        float singleStep = turnSpeed * Time.deltaTime;
        Vector2 newDirection = Vector3.RotateTowards(transform.up, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(transform.forward, newDirection);
    }

    public bool atNextNodePosition(Vector3 _node, Vector3 _guard_pos) => (Vector3.Distance(_node,_guard_pos) < 0.1f);

    IEnumerator processNodes() {
        bool active = true;
        bool forward = true;
        while (active) {
            if (atNextNodePosition(nodes[current_node_position].getPosition,guard_pos.position)) {
                if (enable_debug_messages) {Debug.Log($"Reached node:{current_node_position}");}
                guard_movement_comp.Move(Vector2.zero);
                foreach(GuardNodeActionTypes _action in nodes[current_node_position].actions) {
                    switch (_action)
                    {
                        case GuardNodeActionTypes.DO_NOTHING: break; // :)
                        case GuardNodeActionTypes.TURN_90: guard_pos.Rotate(0,0,90, Space.Self); break;
                        case GuardNodeActionTypes.TURN_180: guard_pos.Rotate(0,0,180, Space.Self); break;
                        case GuardNodeActionTypes.PAUSE_2_SECONDS: yield return new WaitForSeconds(2); break;
                        case GuardNodeActionTypes.PAUSE_5_SECONDS: yield return new WaitForSeconds(5); break;
                        case GuardNodeActionTypes.PAUSE_8_SECONDS: yield return new WaitForSeconds(8); break; // make sure that this action is last in the list of actions!
                        case GuardNodeActionTypes.PAUSE_10_SECONDS: yield return new WaitForSeconds(10); break;
                        default: throw new ArgumentException($"processNodes Coroutine does not have valid case for GuardNodeActionType of type: {_action}");
                    }
                }
                if ((current_node_position == nodes.Count-1 && forward) || (current_node_position == 0 && !forward)) {forward = !forward;}
                if (nodes.Count > 1) {current_node_position += forward ? 1 : -1;}
            } else {
                Vector3 target = nodes[current_node_position].getPosition - guard_pos.position;
                guard_movement_comp.Move(new Vector2(target.x,target.y));
                guard_movement_comp.Direction(new Vector2(target.x,target.y));
            }
            yield return new WaitForSeconds(.01f);
        }
    }
}
