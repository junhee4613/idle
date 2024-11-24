using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
public class SceneSwitcher : MonoBehaviour
{
    [MenuItem("Scenes/Load Lobby")]
    static void LobbyScene()
    {
        LoadScene("Assets/01.Scenes/Base_scene/Lobby_screen.unity");
    }

    static void LoadScene(string scenePath)
    {
        // 씬 저장 여부 확인 및 경고
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // 지정된 씬 로드
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
