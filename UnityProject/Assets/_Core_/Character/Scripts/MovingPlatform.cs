using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{


    public GameObject Player;

    private bool rootsinc = false;


    private void Update()
    {


        if (rootsinc)
        {

            

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            rootsinc = true;
            
            Player.transform.parent = transform;
           
            
          
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == Player)
        {
            rootsinc = false;
            Player.transform.parent = null;

        }

    }



}
