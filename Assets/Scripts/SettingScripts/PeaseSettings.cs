using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Packman-Like_Project/PeaseSettings", fileName = "PeaseSettings")]
public class PeaseSettings : ScriptableObject
{
    public GameObject Prefab;
    public int StartPeaseCount;
    [Tooltip("����� �� �������� �� �������� ����� �������� �������������")]
    [Space]
    public bool NeedSpawnPerTime;
    [Tooltip("�����, ����� ������� ��������� ����� ��������. ��� ��������� � 0 ����� ���������� ������ FixedUpdate")]
    public float SpawnRate;
    [Tooltip("���-�� �������, ������� ��������� �� ����� SpawnRate")]
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
