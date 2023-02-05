using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header ("Movement")]

    //inputs
                                                                        
    //moveTowards Camera
    public Transform cam;
    private float targetAngle;
    private float angle;

    public CharacterController controller;

    public Animator Anim;

    public bool canMove = true;

    public float speed;

    public float turnSmooth = 0.06f;

    public float smoothVel;

    public float walkSpeed;

    public float runSpeed;

    public Vector3 direction;

    public float jumpHeight;



    [Header("Gravity && GroundCheck")]

    //Ground-Gravity

    

    
    public float gravity;
    public float customGravity;
    public float groundDistance;
    

    public LayerMask groundMask;
    public GameObject Root;
    private Vector3 velocityGravity;


    [Header("Climbing System")]

    //Climb

    public bool isClimbable;
    public bool climbing;
    public float climbDistance;
    public float climbSpeed;
    
    public LayerMask climbMask; 
    public GameObject Climb;



    [Header("States")]

    //States

    public bool stunned = false;
    public bool dashing = false;



    [Header("MouseAim")]
    
    //aimmouse

    private Camera mainCamera;
   
    public float aimStop;
    public float aimspeed;
    public LayerMask aimMouseMask;



    [Header("Dash")]

    //dash

    public float dashSpeed;
    public float dashTime;
    private float dashAngle;
    public Vector3 dashDirection;

    //WallJump
    public float wallJumpSpeed;
    public float wallJumpTime;


    //Crouching
    public bool crouching;

    //sitting 
    public bool canSit;
    public bool sitting;
    public GameObject asiento;

    //VectorsMovement


    public Vector3 attackDirection;
    public Vector3 hit_vec = -Vector3.forward;

    //-----------------------------------------------------------------------------------------------------

    private void Awake()
    {

    }

    private void Start()
    {
        canMove = true;
        mainCamera = FindObjectOfType<Camera>();
    }



    
    void Update()
    {
        
        Move();

        
 
        if (Input.GetKeyDown(KeyCode.Space) && !PlayerStatics.aimMouse)
        {
            if (climbing)
            {
                    Anim.SetBool("Jump", true);
                    Anim.speed = 1f;
                    turnSmooth = 0;
                    StartCoroutine(ClimbJump());
                    StartCoroutine(WallJumpMove());
                


            
            }else if(!climbing && isClimbable)
            {   
                Anim.SetBool("ClimbDown", false);
                Anim.SetBool("Jump", false);
                canMove = false;
                StartCoroutine(StartClimb());
                climbing = true;
                gravity = 0f;
                velocityGravity.y = 0f;
                Anim.SetBool("Climb", true);
                
                Debug.Log("Climbing");
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!dashing && !climbing) {

                turnSmooth = 0;
                StartCoroutine(DashTurnDelay());
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canSit)
            {
                if (!sitting) {

                    sitting = !sitting;
                    direction = Vector3.zero;
                    canMove = false;
                    Anim.SetBool("Sit", true);  
                    
                }
                else
                {
                    canMove = true;
                    Anim.SetBool("Sit", false);
                    sitting = !sitting;
                }

            }
        }



    

       

    }


    private void Move()
    {

        //Axis inputs

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!PlayerStatics.aimMouse && canMove && !sitting)
        {


            direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f && !stunned)
            {
                    //cam.euler is for cam angle
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSmooth);
                    //applies the cam transform to the direction
                direction = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;

                if (!climbing && !dashing)
                {
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    
                }
            }

        }
        else
        {  
            if (PlayerStatics.aimMouse && !sitting) { AimMouse(); }
        }

        CanMoveCheck();

        MovementAnimations();

        CanClimb();
    }


    private void MovementAnimations()
    {
        //Movement Animation Managment

        if (PlayerStatics.isGrounded && !stunned)
        {

            Anim.SetBool("Fall", false);

            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {

                Run();

            }
            else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
                

            }
            else if (direction == Vector3.zero)
            {


                
                Idle();
                
            }

            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (PlayerStatics.isGrounded && !climbing && !PlayerStatics.aimMouse)
                {

                    
                    Jump();


                }
       
            }
        }
        else
        {
            if (climbing)
            {
                speed = 0;
                Climbing();
            }
            else
            {
                Fall();
                speed = runSpeed;
                
            }
        }
    }



    private void CanClimb()
    {


        isClimbable = Physics.CheckSphere(Climb.transform.position, climbDistance, climbMask);


        if (!isClimbable )
        {

            
            canMove = true;
            climbing = false;
            gravity = customGravity;
            Anim.SetBool("Climb", false);

        }
        
      

    }


    private void CanMoveCheck()
    {

        //Secure Player grounds properly

        PlayerStatics.isGrounded = Physics.CheckSphere(Root.transform.position, groundDistance, groundMask);

        if (PlayerStatics.isGrounded && velocityGravity.y < 0)
        {
            Anim.SetBool("Jump", false);
            velocityGravity.y = -5f;

        }


        if (!stunned)
        {

            if (canMove)
            {
                controller.Move(direction * speed * Time.deltaTime);
            }

            velocityGravity.y += gravity * Time.deltaTime;

            controller.Move(velocityGravity * Time.deltaTime);


        }
        else
        {

            Idle();

        }

        if(PlayerStatics.isGrounded && !climbing)
        {

            //turnSmooth = 0.06f;
        }

    }


    private void AimMouse()
    {
       
        Plane rayHit = new Plane(Vector3.up, Vector3.zero);
        

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, aimMouseMask))
        {
            if (canMove && !climbing) {

                float distance = Vector3.Distance(Root.transform.position, new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));

                if (distance > aimStop) { 

                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z) - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimspeed * Time.deltaTime);
                }
                else
                {
                    direction = Vector3.zero;
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
        Anim.SetBool("Jump",true);

    }
    private void Fall()
    {
        Anim.speed = 1f;
        //Debug.Log("Falling");
        Anim.SetBool("Fall", true);

    }

    private void Climbing()
    {

        //Debug.Log("climbing");
        Anim.SetBool("Climb", true);
        //turnSmooth = 0.06f;


    }

   

    private IEnumerator StartClimb()
    {
        
        yield return new WaitForSeconds(0.2f);
        Anim.speed = 0f;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("inn");
        if (other.gameObject.name == "asiento")
        {
            canSit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("inn");
        if (other.gameObject.name == "asiento")
        {
            canSit = false;
        }
    }







    IEnumerator Dash()
    {

        Debug.Log("dash");
        if(direction != Vector3.zero && !climbing) {

            velocityGravity.y = 0f;
            StartCoroutine(DashAnim());

           
            float startTime = Time.time;
            while(Time.time < startTime + dashTime)
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
                gravity = 0;
                yield return null;

            }
        }
        else if(climbing)
        {

            dashing = false;

            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);

                yield return null;

            }
        }
        else
        {
            dashing = false;
            turnSmooth = 0.08f;
        }
       
        

    }

    private void Dashdelay()
    {
        dashing = true;
        dashDirection = direction;
        dashAngle = angle;
        
        StartCoroutine(Dash());

        Debug.Log("Dash_update");

    }

    private IEnumerator DashTurnDelay()
    {

        yield return new WaitForSeconds(0.01f);
        Dashdelay();


    }

   

    private IEnumerator DashAnim()
    {


        Anim.SetBool("Dash", true);
        dashing = true;

        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("Dash", false);
        yield return new WaitForSeconds(0.1f);
        dashing = false;
        
        yield return new WaitForSeconds(0.1f);
        turnSmooth = 0.08f;

    }
    private IEnumerator ClimbJump()
    {
        Anim.SetBool("ClimbDown", false);
        canMove = false;
        transform.localRotation *= Quaternion.Euler(0, 180, 0);
        
        

        float startTime = Time.time;
        while (Time.time < startTime + wallJumpTime)
        {

                direction = transform.forward;
                controller.Move(direction * wallJumpSpeed * Time.deltaTime);
                controller.Move(transform.up * 8 * Time.deltaTime);

                yield return null; 

        }

        
        
    }

    private IEnumerator WallJumpMove()
    {
        turnSmooth = 0.2f;
        yield return new WaitForSeconds(0.5f);
        turnSmooth = 0.06f;

    }

   

   





}
