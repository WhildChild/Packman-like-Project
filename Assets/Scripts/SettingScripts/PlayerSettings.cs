using UnityEngine;
[CreateAssetMenu(menuName = "Packman-Like_Project/PlayerSettings", fileName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public GameObject Prefab;
    [Tooltip("Индексы клетки в двумерном массиве всего поля")]
    public Vector2Int SpawnPosition;

    private void OnValidate()
    {
        if (SpawnPosition.x < 0)
        {
            SpawnPosition.x = 0;
        }

        if (SpawnPosition.y < 0) 
        {
            SpawnPosition.y = 0;
        }
    }
}
