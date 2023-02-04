using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlatforms : MonoBehaviour
{
  
    public bool platformState;
    public bool platformtrigger;
    public GameObject platform;
    public float platformTime = 20f;
    // Update is called once per frame

    private void Start()
    {

        platformState = false;
        platformTime = 2f;

    }


    void Update()
    {

            if (platformState)
            {

                

            }

    }



    public void OnTriggerEnter(Collider other)
    {

            if (other.gameObject.name == "Player")
            {
                //this.gameObject.SetActive(false);
                platformState = true;
                   Debug.Log("trigger");
                    StartCoroutine(PlatformDisapear());
            }

    }




    IEnumerator PlatformDisapear()
    {
    
               
                yield return new WaitForSeconds(platformTime);
                DesPlat();
    }

    void DesPlat()
    {
        
       
        StartCoroutine(PlatformRespawn());
        
        Debug.Log("hola");

        
        platform.gameObject.SetActive(false);


    }


    void ActPlat()
    {
        platform.gameObject.SetActive(true);
        platformState = false;

    }
    IEnumerator PlatformRespawn()
    {

            yield return new WaitForSeconds(2f);
                ActPlat();




    }


}
