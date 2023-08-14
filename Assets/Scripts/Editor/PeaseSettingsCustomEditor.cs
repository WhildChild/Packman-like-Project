using UnityEditor;

[CustomEditor(typeof(PeaseSettings))]
public class PeaseSettingsCustomEditor : Editor
{
    private SerializedProperty needSpawnPerTime;
    private SerializedProperty prefab;
    private SerializedProperty startPeaseCount;

    private SerializedProperty spawnRate;
    private SerializedProperty peaseSpawnCountPerTick;


    private void OnEnable()
    {
        needSpawnPerTime = serializedObject.FindProperty("NeedSpawnPerTime");
        prefab = serializedObject.FindProperty("Prefab");
        startPeaseCount = serializedObject.FindProperty("StartPeaseCount");

        spawnRate = serializedObject.FindProperty("SpawnRate");
        peaseSpawnCountPerTick = serializedObject.FindProperty("PeaseSpawnCountPerTick");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(prefab);
        EditorGUILayout.PropertyField(startPeaseCount);
        EditorGUILayout.PropertyField(needSpawnPerTime);

        if (needSpawnPerTime.boolValue)
        {
            EditorGUILayout.PropertyField(spawnRate);
            EditorGUILayout.PropertyField(peaseSpawnCountPerTick);
        }

        serializedObject.ApplyModifiedProperties();
    }
}