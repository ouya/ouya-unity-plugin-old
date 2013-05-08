using UnityEngine;

public class OuyaSceneGame : MonoBehaviour
{
    public string NextScene = "SceneMain";
    public string SplashScene = "SceneSplash";

    public void OnGUI()
    {
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Back to Main Scene", GUILayout.MinHeight(60)))
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