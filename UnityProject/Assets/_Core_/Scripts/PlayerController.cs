using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{ 
    const int _maxCollectables = 10;
    int _nCollectables;
    Environments _collectableGathered;
    CollectableBehaviour _currentCollidingCollectable;

    public UnityAction OnCollectableRangeEnter;
    public UnityAction OnCollectableRangeExit;

    private void Start()
    {
        _nCollectables = 0;
        _collectableGathered = Environments.NONE;
        _currentCollidingCollectable = null;
    }

    public void ResetGatheredCollectables()
    {
        _nCollectables = 0;
        _collectableGathered = Environments.NONE;
    }

    void GatherCollectable(Environments c)
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

    public bool IsInGatheringRange()
    {
        return _currentCollidingCollectable is not null;
    }

    private void Update()
    {
        if (_currentCollidingCollectable != null && Input.GetKey(KeyCode.E))
        {
            // Gather collectable
            GatherCollectable(_currentCollidingCollectable.Type);
            _currentCollidingCollectable.Gather();

            OnCollectableRangeExit.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableBehaviour c = other.GetComponent<CollectableBehaviour>();
        // Show message
        // enable gathering
        _currentCollidingCollectable = c;
        OnCollectableRangeEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        CollectableBehaviour c = other.GetComponent<CollectableBehaviour>();

        if (c == _currentCollidingCollectable)
        {
            c = null;
            OnCollectableRangeExit.Invoke();
        }
    }

}
