using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    const int _maxCollectables = 10;
    int _nCollectables;
    CollectableTypes _collectableGathered;

    private void Start()
    {
        _nCollectables = 0;
        _collectableGathered = CollectableTypes.NONE;
    }

    public void ResetGatheredCollectables()
    {
        _nCollectables = 0;
        _collectableGathered = CollectableTypes.NONE;
    }

    void GatherCollectable(CollectableTypes c)
    {
        if (c != _collectableGathered)
        {
            _nCollectables = 0;
            _collectableGathered = c;
        }

        _nCollectables += 1;

        if (_nCollectables > _maxCollectables)
            _nCollectables = _maxCollectables;
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableBehaviour c = other.GetComponent<CollectableBehaviour>();
        // Show message
        // enable gathering
        GatherCollectable(c.Type);
        c.Gather();
    }

}
