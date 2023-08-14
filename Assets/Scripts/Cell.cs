using Unity.VisualScripting;
using UnityEngine;

public class Cell 
{
    public GameObject GameObject;

    public int x, y;

    public GameObject RightWall ;
    public GameObject LeftWall ;
    public GameObject TopWall ;
    public GameObject BottomWall ;

    public bool IsVisited;

    public GameObject Pease;

    public CellStatus CellStatus;   
}
public enum CellStatus
{
    Empty,
    HavePlayer,
    HavePease,
    HaveEnemy
}