using UnityEditor;

[CustomEditor(typeof(MazeSettings))]
public class MazeSettingsCustomEditor : Editor
{
    private SerializedProperty _groundPrefab;
    private SerializedProperty _wallPrefab;
    private SerializedProperty _wallsCount;

    private SerializedProperty _sideSize;
    

    private void OnEnable()
    {
        _groundPrefab = serializedObject.FindProperty("GroundPrefab");
        _wallPrefab = serializedObject.FindProperty("WallPrefab");
        _wallsCount = serializedObject.FindProperty("WallsCount");

        _sideSize = serializedObject.FindProperty("SideSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_wallPrefab);
        EditorGUILayout.PropertyField(_groundPrefab);

        EditorGUILayout.Space(25);

        int maxWallsCount = _sideSize.intValue * (_sideSize.intValue - 1);
        if (_wallsCount.intValue > maxWallsCount  )
            _wallsCount.intValue = maxWallsCount;

        EditorGUILayout.IntSlider(_wallsCount, 0, maxWallsCount);
        EditorGUILayout.IntSlider(_sideSize, 5, 20);

        serializedObject.ApplyModifiedProperties();
    }
}