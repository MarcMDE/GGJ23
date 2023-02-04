using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour
{


    public Camera mainCamera;
    public LayerMask groundMask;


    void Update()
    {

        AimMouse();

    }

    private void AimMouse()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundMask))
        {

            transform.position = new Vector3 (raycastHit.point.x, raycastHit.point.y , raycastHit.point.z);
        }

    }
}
