using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "bullet(Clone)" )
        {

            Destroy(this.gameObject, 0.1f);
            Destroy(other.gameObject, 0.1f);

        }
    }

}
