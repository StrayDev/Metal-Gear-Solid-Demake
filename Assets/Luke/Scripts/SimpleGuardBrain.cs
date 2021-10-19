using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleGuardBrain : MonoBehaviour
{
    // Start is called before the first frame update
    private int current_node_position = 0;
    private Transform guard_pos;
    private MoveComponent guard_movement_comp;

    [SerializeField] private bool enable_debug_messages = false;
    [SerializeField] private List<GuardNode> nodes;
    void Awake() {
        guard_movement_comp = GetComponent<MoveComponent>();
        guard_pos = GetComponent<Transform>();
    }
    void Start()
    {
        StartCoroutine("processNodes");
    }

    public bool atNextNodePosition(Vector3 _node, Vector3 _guard_pos) => (Vector3.Distance(_node,_guard_pos) < 0.01);

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
