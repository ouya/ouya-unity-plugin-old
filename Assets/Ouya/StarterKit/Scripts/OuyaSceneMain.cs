using UnityEngine;

public class OuyaSceneMain : MonoBehaviour
{
    public string NextScene = "SceneGame";
    public string SplashScene = "SceneSplash";

    public void OnGUI()
    {
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Load the Game Scene", GUILayout.MinHeight(60)))
        {
            Application.LoadLevel(NextScene);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Reset to Splash Screen", GUILayout.MinHeight(60)))
        {
            Application.LoadLevel(SplashScene);
        }
        GUILayout.EndHorizontal();
    }
}