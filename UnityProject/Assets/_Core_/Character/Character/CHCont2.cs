using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHCont2 : MonoBehaviour
{


    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right;

    //Movement

    public CharacterController controller;

    public Animator Anim;

    public float speed;

    public float turnSmooth = 0.1f;

    public float smoothVel;

    public float walkSpeed;

    public float runSpeed;

    public Material player_mat;

    public float jumpHeight;


    //Ground-Gravity

    public bool isGrounded = true;
    public float groundDistance;
    public LayerMask groundMask;
    public Vector3 velocityGravity;

    public float gravity;
    public GameObject Root;



    //VectorsMovement

    public Vector3 direction;
    public Vector3 attackDirection;
    public Vector3 hit_vec = -Vector3.forward;



    //States
    public bool stunned = false;
    public bool dash = false;



    //aimmouse
    private Camera mainCamera;
    public bool aimMouse;


    //Tests

    public float dashSpeed;
    public float dashTime;






    private void Awake()
    {

    }

    private void Start()
    {
         forward = Camera.main.transform.forward;
         forward.y = 0;
         forward = Vector3.Normalize(forward);
         right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
       
         mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {

            StartCoroutine(Dash());

            Debug.Log("Dash_update");

        }

        Move();

        //Dash();

    }


    private void Move()
    {

        //Axis inputs

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        
            direction = new Vector3(horizontal, 0, vertical).normalized;
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            transform.forward = heading;
            transform.position += rightMovement;
            transform.position += upMovement;
        

        

    }


    private void MovementAnimations()
    {
        //Movement Animation Managment

        if (isGrounded && !stunned)
        {

            Anim.SetBool("Fall", false);

            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {

                Walk();

            }
            else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {

                Run();

            }
            else if (direction == Vector3.zero)
            {



                Idle();

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

                Jump();

            }



        }
        else
        {
            Fall();
            speed = runSpeed;
        }


    }
    private void CanMoveCheck()
    {

        //Secure Player grounds properly

        isGrounded = Physics.CheckSphere(Root.transform.position, groundDistance, groundMask);

        if (isGrounded && velocityGravity.y < 0)
        {
            Anim.SetBool("Jump", false);
            velocityGravity.y = -4f;

            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            transform.forward = heading;
            transform.position += rightMovement;
            transform.position += upMovement;

        }

    }
    private void AimMouse()
    {
        if (direction.magnitude >= 0.1f && !stunned)
        {

            Plane rayHit = new Plane(Vector3.up, Vector3.zero);
            if (!aimMouse)
            {


                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref smoothVel, turnSmooth);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                float rayLenght;
                if (rayHit.Raycast(ray, out rayLenght))
                {
                    Vector3 point = ray.GetPoint(rayLenght);
                    Debug.DrawLine(ray.origin, point, Color.red);
                    transform.LookAt(new Vector3(point.x, transform.position.y, point.z));



                    float targetAngle = Mathf.Atan2(point.x, point.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref smoothVel, turnSmooth);

                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    direction = transform.forward;
                    Debug.Log("Forward");
                }
            }


        }
        else
        {

            Plane rayHit = new Plane(Vector3.up, Vector3.zero);
            if (!aimMouse)
            {

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref smoothVel, turnSmooth);

                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
            else
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                float rayLenght;
                if (rayHit.Raycast(ray, out rayLenght))
                {
                    Vector3 pointToLook = ray.GetPoint(rayLenght);
                    Debug.DrawLine(ray.origin, pointToLook, Color.red);
                    transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));


                    float targetAngle = Mathf.Atan2(pointToLook.x, pointToLook.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref smoothVel, turnSmooth);

                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    direction = transform.forward;
                    Debug.Log("Forward2");
                }

            }
        }
    }



    //Animations
    private void Idle()
    {

        speed = 0f;
        Anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);

    }

    private void Walk()
    {
        speed = walkSpeed;
        Anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);


    }

    private void Run()
    {
        speed = runSpeed;
        Anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);

    }

    private void Jump()
    {

        velocityGravity.y = Mathf.Sqrt(jumpHeight * -1.5f * gravity);
        Debug.Log("Jump");
        Anim.SetBool("Jump", true);

    }
    private void Fall()
    {

        Debug.Log("Falling");
        Anim.SetBool("Fall", true);

    }





    IEnumerator Dash()
    {
        if (direction != Vector3.zero)
        {


            StartCoroutine(DashAnim());

            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {

                controller.Move(direction * dashSpeed * Time.deltaTime);

                yield return null;
            }
        }

    }

    private IEnumerator DashAnim()
    {


        Anim.SetBool("Dash", true);

        yield return new WaitForSeconds(0.3f);
        Anim.SetBool("Dash", false);

    }



    //Meter esto en el update y hacer una condicion que se active en el attack y en el yeld return ponerla a false....
}
