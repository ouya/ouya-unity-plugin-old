using UnityEngine;
using System.Collections;

public class OuyaExampleGroundRaycast : MonoBehaviour
{

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = Camera.mainCamera.ScreenPointToRay(mousePos);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
    }
}