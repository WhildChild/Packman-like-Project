
using UnityEngine;

public struct PlayerInputEventComponent 
{
    public InputEventType InputEventType;
    public Vector2 direction;
}

public enum InputEventType { Movable }

