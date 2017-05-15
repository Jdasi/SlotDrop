using UnityEditor;
 
[CustomEditor(typeof(PersistantObjects))]
public class PersistantObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Make any objects you want to persist between levels a child of this object." + 
            " Objects that are not a child will be reloaded when the level is reset.", MessageType.Info);   
    }
}

