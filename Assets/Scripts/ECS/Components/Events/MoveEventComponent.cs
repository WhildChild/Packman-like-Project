using UnityEngine;


struct MoveEventComponent
{ 
    public Vector2 Direction;
    public EventAuthorType Author;
}

public enum EventAuthorType {Player, Enemy }
