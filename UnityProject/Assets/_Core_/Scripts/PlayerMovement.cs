using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    [SerializeField]
    private float _playerSpeed = 2.0f;
    [SerializeField]
    private float _jumpForce = 7.5f;
    [SerializeField]
    float _terminalFallSpeed = 10.0f;
    private Vector3 _gravity;

    private void Start()
    {
        _gravity = Physics.gravity;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * Time.deltaTime * _playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

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
    }
}
