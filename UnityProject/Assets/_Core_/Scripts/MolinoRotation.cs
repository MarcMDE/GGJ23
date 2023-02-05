using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolinoRotation : MonoBehaviour
{

	float speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    	this.transform.Rotate(Vector3.forward * speed);

    }
}
