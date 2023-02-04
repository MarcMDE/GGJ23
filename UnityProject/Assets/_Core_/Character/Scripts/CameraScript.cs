using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

        [Header("Atributes")]
         public GameObject character;
        public float smoothVel = 0.5f;
        private Vector3 vectorsmot = Vector3.zero;
        public float distance;

        [Header("Camera Movement Tweaking")]

        [Range(0,10)]
        public float maxDistance = 1f;

        public float maxSmooth;
        public float minSmooth;
        public float IncrementSmoothVel = 0.2f;

    private void Start()
    {
        transform.position = character.transform.position;
        maxDistance = 2f;
        
    }

    private void Update()
    {

        distance = Vector3.Distance(transform.position, character.transform.position );

        if (distance >= 1f)
        {

            if (distance > maxDistance)
            {

                if (smoothVel >= minSmooth)
                {
                    smoothVel -= IncrementSmoothVel;
                }

            }
            else
            {

                if (smoothVel <= maxSmooth)
                {
                    smoothVel += IncrementSmoothVel;
                }

            }
        }

       


    }
    void LateUpdate()    
    {


        //moves to the other side
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-character.transform.position.x, transform.position.y, -character.transform.position.z), ref vectorsmot, smoothVel);

        transform.position = Vector3.SmoothDamp(transform.position, character.transform.position, ref vectorsmot, smoothVel);

    }
    

}
