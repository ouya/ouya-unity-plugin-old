using UnityEngine;

public class OuyaSceneInit : MonoBehaviour
{
    public string NextScene = "SceneSplash";

    public void Start()
    {
        Application.LoadLevel(NextScene);
    }
}