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
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float forward = Input.GetAxis("Vertical");
        float side = Input.GetAxis("Horizontal");


        _controller.Move((_cameraController.transform.forward * forward + _cameraController.transform.right * side) * Time.deltaTime * _playerSpeed);

        // ROTATION
        /*
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        */
        transform.forward = new Vector3(_cameraController.transform.forward.x, 0, _cameraController.transform.forward.z);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _groundedPlayer)
        {
            _playerVelocity.y += _jumpForce;
        }

        _playerVelocity += _gravity * Time.deltaTime;

        if (_playerVelocity.y < -_terminalFallSpeed)
        {
            _playerVelocity.y = -_terminalFallSpeed;
            Debug.Log("Terminal vel");
        }

        _controller.Move(_playerVelocity * Time.deltaTime);

        _cameraController.TargetPosition = transform.position;
    }
}
