using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public ScriptableObject script;



    public void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if(PlayerStatics.crouching == true)
            {
                Shoot();


            }
               
            
        }
        

    }


    public void Shoot()
    {
        Invoke("Generate", 0.1f);

    }

    public void Generate()
    {

        Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        Debug.Log("genera");
    }
}
