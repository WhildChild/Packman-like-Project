using UnityEngine;

[CreateAssetMenu(menuName = "Packman-Like_Project/MazeSettings", fileName = "MazeSettings")]
public class MazeSettings : ScriptableObject
{
    public GameObject GroundPrefab;
    public GameObject WallPrefab;
 
    public int WallsCount;

    public int Width;
    public int Height;

    public float CellSize = 0.5f;

}
