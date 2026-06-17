using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeSceneBootstrap
{
    private const string PersistentScenePath = "Assets/Scenes/PersistentScene.unity";

    static PlayModeSceneBootstrap()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (EditorSceneManager.GetActiveScene().path != PersistentScenePath)
            {
                EditorSceneManager.OpenScene(PersistentScenePath);
            }
        }
    }
}