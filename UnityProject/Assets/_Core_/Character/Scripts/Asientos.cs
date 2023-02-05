using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asientos : MonoBehaviour
{
    public GameObject player;
    public CharacterController playerController;
    public bool canSit;
    public bool sitting = false;
    public float smoothVel = 0.5f;
    private Vector3 vectorsmot = Vector3.zero;

    private void Start()
    {
        sitting = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSit) {

            if (!sitting) {

                playerController.enabled = false;
                player.transform.position = transform.position;
                player.transform.rotation = transform.rotation;
                sitting = !sitting;
                

            }
            else
            {
                sitting = !sitting;
                playerController.enabled = true;
            }
        }

        

        if (Input.GetKeyDown(KeyCode.P))
        {
            
            player.transform.position = new Vector3(0,0,0);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {

            canSit = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

            canSit = false;


        }
    }
}
