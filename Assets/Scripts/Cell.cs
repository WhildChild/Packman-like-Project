using UnityEngine;

public class Cell 
{
    public GameObject GameObject;

    public int wallsCount = 0;

    public int x, y;

    public GameObject RightWall ;
    public GameObject LeftWall ;
    public GameObject TopWall ;
    public GameObject BottomWall ;

    public bool IsVisited;
}
