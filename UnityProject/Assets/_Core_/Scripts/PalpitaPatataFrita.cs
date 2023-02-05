using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalpitaPatataFrita : MonoBehaviour
{
    Transform _meshTransform;
    [SerializeField] float _growAdd = 0.4f;

    [SerializeField] float _speed = 0.5f;

    float _counter, _add;
    Vector3 _originalScale;
    void Start()
    {
        _counter = 0;
        _add = 0;
        _meshTransform = transform.GetChild(0);

        if (_meshTransform is not null)
        {
            _originalScale = _meshTransform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_meshTransform is not null)
        {
            _counter += Time.deltaTime * Mathf.PI * _speed;

            if (_counter > Mathf.PI * 2.0f) _counter = 0.0f;

            _add = Mathf.Sin(_counter) * _growAdd;

            _meshTransform.localScale = _originalScale + Vector3.one * _add;
        }
    }
}
