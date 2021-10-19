using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuardNodeActionTypes {
    DO_NOTHING,
    PAUSE_10_SECONDS,
    PAUSE_8_SECONDS,
    PAUSE_5_SECONDS,
    PAUSE_2_SECONDS,
    TURN_180,
    TURN_90
}
[Serializable] public class GuardNode
{
    public Transform position;
    public List<GuardNodeActionTypes> actions;
    public GuardNode(Transform _position, List<GuardNodeActionTypes> _actions) {
        position = _position;
        actions = _actions;
    }

    public Vector3 getPosition => position.position;
    public List<GuardNodeActionTypes> getActions => actions;
}
