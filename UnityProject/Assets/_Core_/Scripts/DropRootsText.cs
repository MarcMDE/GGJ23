using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropRootsText : MonoBehaviour
{
    [SerializeField]
    CalderoBehaviour _caldero;

    TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_caldero.GetPlayerInZone())
        {
            _text.enabled = true;
        }
        else
        {
            _text.enabled = false;
        }
    }
}
