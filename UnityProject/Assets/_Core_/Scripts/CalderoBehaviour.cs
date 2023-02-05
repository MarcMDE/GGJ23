using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CalderoBehaviour : MonoBehaviour
{
    const int _target = 10;

    int[] _collectablesNum = new int[3];

    [SerializeField] GameObject[] _completeObjects = new GameObject[3];

    [SerializeField] GameObject [] _countPanel = new GameObject[3];

    [SerializeField] Transform _worldCanvas;

    bool _playerInZone = false;


    void Start()
    {
        for (int i=0; i<3; i++)
        {
            _completeObjects[i].SetActive(false);
            _countPanel[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
        }
        
        _collectablesNum[0] = 0;
        _collectablesNum[1] = 0;
        _collectablesNum[2] = 0;
    }

    public void Restart()
    {
        for (int i=0; i<3; i++)
        {
            if (_collectablesNum[i] < _target)
            {
                _collectablesNum[i] = 0;
                _countPanel[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            }
        }
    }

    public bool GameWin()
    {
        bool w = true;
        for (int i = 0; i < 3; i++)
        {
            if (_collectablesNum[i] < _target)
            {
                w = false;
                break;
            }
        }
        return w;
    }

    public void Add(Environments e, int n)
    {
        _collectablesNum[(int)e] += n;
        if (_collectablesNum[(int)e] >= _target)
        {
            _collectablesNum[(int)e] = _target;
            _completeObjects[(int)e].SetActive(true);
            _countPanel[(int)e].SetActive(false);
        }
        else
            _countPanel[(int)e].GetComponentInChildren<TextMeshProUGUI>().text = _collectablesNum[(int)e].ToString();


    }

    public bool GetPlayerInZone()
    {
        return _playerInZone;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var p = other.GetComponent<PlayerController>();

            if (p.GetCurrentCollectable() != Environments.NONE)
            {
                Add(p.GetCurrentCollectable(), p.GetCollectedNum());
                p.GiveAllCollectables();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        _playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerInZone = false;
    }

    private void Update()
    {
        /*
        Camera camera = Camera.main;

        _worldCanvas.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
        */
        _worldCanvas.LookAt(Camera.main.transform.position);
        
    }
}
