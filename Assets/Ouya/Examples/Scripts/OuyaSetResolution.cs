using UnityEngine;
using System.Collections;

public class OuyaSetResolution : MonoBehaviour
{
    private bool m_fullScreen = true;

    public void OnGUI()
    {
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        m_fullScreen = GUILayout.Toggle(m_fullScreen, "FullScreen?", GUILayout.Height(40));
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Set Resolution 480p", GUILayout.Height(40)))
        {
            Screen.SetResolution(640, 480, m_fullScreen);
            OuyaSDK.OuyaJava.JavaSetResolution("640x480");
        }
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Set Resolution 720p", GUILayout.Height(40)))
        {
            Screen.SetResolution(1280, 720, m_fullScreen);
            OuyaSDK.OuyaJava.JavaSetResolution("1280x720");
        }
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Set Resolution 1080p", GUILayout.Height(40)))
        {
            Screen.SetResolution(1920, 1080, m_fullScreen);
            OuyaSDK.OuyaJava.JavaSetResolution("1920x1080");
        }
        GUILayout.EndHorizontal();
    }
}