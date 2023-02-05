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
    public int GetCollectedNum()
    {
        return _nCollectables;
    }

    public Environments GetCurrentCollectable()
    {
        return _collectableGathered;
    }

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

    public void GiveAllCollectables()
    {
        _nCollectables = 0;
        _collectableGathered = Environments.NONE;
    }
    void GatherCollectable()
    {
        if (_currentCollidingCollectable.Type != _collectableGathered)
        {
            _nCollectables = 0;
            _collectableGathered = _currentCollidingCollectable.Type;
        }

        _currentCollidingCollectable.Gather();
        _currentCollidingCollectable = null;

        _nCollectables += 1;

        if (_nCollectables > _maxCollectables)
            _nCollectables = _maxCollectables;

        OnCollectableRangeExit.Invoke();


    }

    public bool IsInGatheringRange()
    {
        return _currentCollidingCollectable is not null;
    }

    private void Update()
    {
        if (_currentCollidingCollectable is not null && Input.GetKeyDown(KeyCode.E))
        {
            // Gather collectable
            GatherCollectable();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "col")
        {
            CollectableBehaviour c = other.GetComponent<CollectableBehaviour>();
            // Show message
            // enable gathering
            _currentCollidingCollectable = c;
            OnCollectableRangeEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "col")
        {
            CollectableBehaviour c = other.GetComponent<CollectableBehaviour>();

            if (c == _currentCollidingCollectable)
            {
                _currentCollidingCollectable = null;
                OnCollectableRangeExit.Invoke();
            }

        }
    }

}
