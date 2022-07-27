using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(AudioEvent)), true)]
public class AudioEventEditor : Editor
{
    [SerializeField] private AudioSource preListener;

    public void OnEnable()
    {
        preListener = EditorUtility
            .CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource))
            .GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(preListener.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if(GUILayout.Button("Preview"))
        {
            ((AudioEvent) target).Play(preListener);
        }
        EditorGUI.EndDisabledGroup();
    }
}
