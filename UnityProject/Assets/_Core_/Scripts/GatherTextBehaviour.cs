using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherTextBehaviour : MonoBehaviour
{
    PlayerController _playerController;
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gameObject.SetActive(false);

        _playerController.OnCollectableRangeEnter += Show;
        _playerController.OnCollectableRangeExit += Hide;
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
