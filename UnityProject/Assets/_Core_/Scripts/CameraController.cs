using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _mouseSensitivity = 800.0f;

    float _xRotation = 0f;
    float _yRotation = 0f;

    public Vector3 TargetPosition { get; set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        transform.position = TargetPosition;
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation += mouseX;
        _yRotation -= mouseY;
        _yRotation = Mathf.Clamp(_yRotation, -50f, 50f);
        //_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        //yRotation = Mathf.Clamp(yRotation, -42f, 42f);

        transform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);

    }
}
