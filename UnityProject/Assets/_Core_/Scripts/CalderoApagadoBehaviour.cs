using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace GGJ23
{
    public class CalderoApagadoBehaviour : MonoBehaviour
    {
        [SerializeField]
        GameManager _gManager;

        [SerializeField] TextMeshProUGUI _text;

        bool _playerInZone = false;

        public bool GetPlayerInZone()
        {
            return _playerInZone;
        }

        void Start()
        {
            _text.enabled = false;
        }

        void Update()
        {
        
        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // TODO: regen
                //_gManager.Regenerate();
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            _text.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _text.enabled = false;
        }
    }

}
