using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    Environments _type;

    public Environments Type { get { return _type; }}

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Gather()
    {
        Destroy(gameObject);
    }
}
