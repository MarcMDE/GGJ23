using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatics 
{

    //inputs

    public static bool canMove = true;

    //Ground-Gravity

    public static bool isGrounded = true;
   
    //Climb

    public static bool isClimbable;
    public static bool climbing;
 
    //States

    public static bool stunned = false;
    public static bool dashing = false;

    //aimmouse

    public static bool aimMouse;

 
    //Crouching
    public static bool crouching;

    //sitting 
    public static bool canSit;
    public static bool sitting;

 
}
