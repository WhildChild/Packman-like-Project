using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Packman-Like_Project/PeaseSettings", fileName = "PeaseSettings")]
public class PeaseSettings : ScriptableObject
{
    public GameObject Prefab;
    public int StartPeaseCount;
    [Tooltip("Ќужно ли спавнить со временем новые горошины автоматически")]
    [Space]
    public bool NeedSpawnPerTime;
    [Tooltip("¬рем€, через которое спавн€тс€ новые горошины. ѕри установке в 0 спавн происходит каждый FixedUpdate")]
    public float SpawnRate;
    [Tooltip(" ол-во горошин, которые спавн€тс€ за врем€ SpawnRate")]
    public int PeaseSpawnCountPerTick;

    private void OnValidate()
    {
        if (SpawnRate < 0) 
        {
            SpawnRate = 0;
        }
        if (PeaseSpawnCountPerTick < 0)
        {
            PeaseSpawnCountPerTick = 0;
        }

        if (StartPeaseCount < 0)
        {
            StartPeaseCount = 0;
        }
    }  
}
