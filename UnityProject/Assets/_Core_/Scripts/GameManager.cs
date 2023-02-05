using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        Transform _character, _cameraController;

        [SerializeField]
        WorldManager _worldManager;

        void Start()
        {
            _character.gameObject.SetActive(false);
            _worldManager.OnWorldReady += StartGame;
        }

        void StartGame()
        {
            _character.position = _worldManager.GetInitialPosition() + Vector3.up * 3;
            _character.gameObject.SetActive(true);
        }

        void Update()
        {
        
        }
    }
}
