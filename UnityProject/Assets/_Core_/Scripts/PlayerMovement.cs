using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    CameraController _cameraController;
    [SerializeField]
    private float _playerSpeed = 2.0f;
    [SerializeField]
    private float _jumpForce = 7.5f;
    [SerializeField]
    float _terminalFallSpeed = 10.0f;
    [SerializeField]
    float _airSpeedFactor = 1.5f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Vector3 _gravity;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = Physics.gravity;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (_controller.isGrounded)
        {
            // Ensure grounded
            _playerVelocity.y = -_controller.stepOffset / Time.deltaTime;
        }
        else
        {
            // Apply gravity
            _playerVelocity += _gravity * Time.deltaTime;
            
            // Terminal falling velocity
            if (_playerVelocity.y < -_terminalFallSpeed)
            {
                _playerVelocity.y = -_terminalFallSpeed;
                //Debug.Log("Terminal vel");
            }
        }

        // Apply gravity
        _controller.Move(_playerVelocity * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            //_playerVelocity.y += _jumpForce;
            _playerVelocity.y = _jumpForce;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        // Character movement (X,Z) ----------------
        float forward = Input.GetAxis("Vertical");
        float side = Input.GetAxis("Horizontal");

        Vector3 planeVelocity = (_cameraController.transform.forward * forward + _cameraController.transform.right * side) * _playerSpeed;
        planeVelocity.y = 0;

        // Extra speed on air
        if (!_controller.isGrounded)
            planeVelocity *= _airSpeedFactor;

        _controller.Move(planeVelocity * Time.deltaTime);
        // ----------------------------------------

        // Character rotation
        transform.forward = new Vector3(_cameraController.transform.forward.x, 0, _cameraController.transform.forward.z);

        // Set camera position
        _cameraController.TargetPosition = transform.position;
    }
}
