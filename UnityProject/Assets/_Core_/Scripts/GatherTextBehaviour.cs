using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherTextBehaviour : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;
    void Start()
    {
        _playerController.OnCollectableRangeEnter += Show;
        _playerController.OnCollectableRangeExit += Hide;
        gameObject.SetActive(false);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
    }
}
