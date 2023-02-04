using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimPlane : MonoBehaviour
{
    public GameObject root;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3 (root.transform.position.x, root.transform.position.y -0.5f, root.transform.position.z);
        
    }
}
