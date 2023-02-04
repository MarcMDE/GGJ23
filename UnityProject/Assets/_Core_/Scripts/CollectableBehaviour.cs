using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableTypes { NEUTRAL=0, AIR, BLOCKED, NONE };

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    CollectableTypes _type;

    public CollectableTypes Type { get { return _type; }}

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
