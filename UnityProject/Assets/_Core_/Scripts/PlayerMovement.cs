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

    Animator Anim;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = Physics.gravity;

        Anim = GetComponentInChildren<Animator>();
    }

    //Animations
    private void Idle()
    {

        //speed = 0f;
        if (Anim is not null)
        {
            Anim.SetBool("Jump", false);
            Anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
        print("Idle");

    }

    private void Walk()
    {
        //speed = walkSpeed;
        if (Anim is not null)
        {
            Anim.SetBool("Jump", false);
            Anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        print("Walk");

    }

    private void Run()
    {
        //speed = runSpeed;
        if (Anim is not null)
        {
            Anim.SetBool("Jump", false);
            Anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        }

        print("Run");
    }

    private void Jump()
    {

        //velocityGravity.y = Mathf.Sqrt(jumpHeight * -1.5f * gravity);
        Debug.Log("Jump");
        if (Anim is not null) Anim.SetBool("Jump", true);

    }
    private void Fall()
    {
        Debug.Log("Falling");
        if (Anim is not null)
        {
            Anim.speed = 1f;
            Anim.SetBool("Fall", true);

        }

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

        if (_controller.isGrounded)
        {
            if (Anim is not null) Anim.SetBool("Fall", false);

            if (Input.GetButtonDown("Jump"))
            {
                //_playerVelocity.y += _jumpForce;
                _playerVelocity.y = _jumpForce;
                _controller.Move(_playerVelocity * Time.deltaTime);
                Jump();
            }


        }
        else if (_playerVelocity.y < 0)
        {
            Fall();
        }

        // Jump

        // Character movement (X,Z) ----------------
        float forward = Input.GetAxis("Vertical");
        float side = Input.GetAxis("Horizontal");

        Vector3 planeVelocity = (_cameraController.transform.forward * forward + _cameraController.transform.right * side) * _playerSpeed;
        planeVelocity.y = 0;

        // Extra speed on air
        if (!_controller.isGrounded)
            planeVelocity *= _airSpeedFactor;

        if (_controller.isGrounded)
        {
            if (planeVelocity.magnitude > 0)
            {
                Run();
            }
            else
            {
                Idle();
            }
        }


        _controller.Move(planeVelocity * Time.deltaTime);

        //if (planeVelocity.magnitude > 0 && _controller.isGrounded) Run();
        //else Idle();
        // ----------------------------------------
        
        // Character rotation
        transform.forward = new Vector3(_cameraController.transform.forward.x, 0, _cameraController.transform.forward.z);

        // Set camera position
        _cameraController.TargetPosition = transform.position;
    }
}
