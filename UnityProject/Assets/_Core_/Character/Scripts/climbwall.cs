using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbwall : MonoBehaviour
{

    

    public GameObject player;
    private bool inside = false;
    private float angley;



    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.Space) && inside)
        {
            
            player.GetComponent<CharacterController>().enabled = false;

            player.transform.rotation = transform.rotation;

            player.transform.parent = transform;

            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, 0);

            player.GetComponent<CharacterController>().enabled = true;

            player.transform.parent = null;

            inside = false;

        }

       
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {

            inside = true;
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

            inside = false;
            //PlayerStatics.climbing = false;
        }
    }
}
