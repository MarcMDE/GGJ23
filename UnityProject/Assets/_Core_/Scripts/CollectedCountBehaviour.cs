using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;


public class CollectedCountBehaviour : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField] Sprite[] _sprites = new Sprite[3];

    [SerializeField]
    TextMeshProUGUI _countText;
    [SerializeField]
    Image _image;

    bool _active;

    void Start()
    {
        _active = false;
        _countText.enabled = false;
        _image.enabled = false;
    }

    void Update()
    {
        if (_active && _playerController.GetCurrentCollectable() == Environments.NONE)
        {
            _active = false;
            _countText.enabled = false;
            _image.enabled = false;
        }
        else if (!_active && _playerController.GetCurrentCollectable() != Environments.NONE)
        {
            _active = true;

            _countText.enabled = true;
            _image.enabled = true;
            //_image.sprite = _sprites[(int)_playerController.GetCurrentCollectable()];
        }

        if (_active)
        {
            _countText.text = _playerController.GetCollectedNum().ToString();
        }
    }
}
