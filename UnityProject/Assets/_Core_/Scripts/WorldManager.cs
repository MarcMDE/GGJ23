using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        int _nProps;

        [SerializeField]
        GameObject[] _propsAutum;
        [SerializeField]
        GameObject[] _propsWinter;
        [SerializeField]
        GameObject[] _propsSpring;

        [SerializeField]
        GameObject _calderoParent;

        CalderoBehaviour _caldero;
        CalderoApagadoBehaviour[] _calderosApagados;

        [SerializeField]
        GameObject[] _collectables = new GameObject[3];

        GameObject[][] _props = new GameObject[3][];

        bool first = true;

        GenerateChunkLoop _generateChunkLoop;
        ColorRoots _colorRoots;
        PropsSpawner _propsSpawner;

        public UnityAction OnWorldReady;

        public Vector3 GetInitialPosition()
        {
            return _generateChunkLoop.Meshes[0, 0, 0].transform.position + Vector3.one * _generateChunkLoop.ChunkSize * 0.5f;
        }

        public Vector3 GetPosition(GameObject m)
        {
            return m.transform.position + Vector3.one * _generateChunkLoop.ChunkSize * 0.5f;

        }

        void Start()
        {
            _caldero = _calderoParent.GetComponentInChildren<CalderoBehaviour>();
            _calderosApagados = _calderoParent.GetComponentsInChildren<CalderoApagadoBehaviour>();

            _caldero.gameObject.SetActive(false);
            for (int i=0; i<_calderosApagados.Length; i++)
            {
                _calderosApagados[i].gameObject.SetActive(false);
            }

            _generateChunkLoop = GetComponent<GenerateChunkLoop>();
            _colorRoots = GetComponent<ColorRoots>();
            _propsSpawner = GetComponent<PropsSpawner>();

            _props[(int)Environments.AUTUM] = _propsAutum;
            _props[(int)Environments.WINTER] = _propsWinter;
            _props[(int)Environments.SPRING] = _propsSpring;

            _generateChunkLoop.OnMeshesReady += AfterMeshLoaded;

            _generateChunkLoop.Generate();
        }

        public void AfterMeshLoaded()
        {
            var meshes = _generateChunkLoop.Meshes;
            int i=0;
            foreach (var m in meshes)
            {
                if (i > 0 || first)
                {
                    Environments e1, e2;
                    GetRandomEnvironments(out e1, out e2);
                    // TODO: Calc influence
                    float rEnv = Random.Range(0.2f, 0.8f);
                    _colorRoots.Paint(rEnv, 1 - rEnv, e1, e2, m);

                    // TODO: Spawn environment collectibles only on its environmen
                    _propsSpawner.Spawn(m, 4, _collectables[(int)e1]);
                    
                    // TODO: Spawn environment props only on its environment
                    foreach (var p in _props[(int)e1])
                    {
                        _propsSpawner.Spawn(m, _nProps, p);
                    }
                    foreach (var p in _props[(int)e2])
                    {
                        _propsSpawner.Spawn(m, _nProps, p);
                    }

                    
                    
                    // TODO: Spawn roots under (and paint :/)
    
                }
            i++;            
            }

            
            int k = 0;
            foreach(var m in meshes)
            {
                if (k>0)
                {
                    _calderosApagados[k-1].transform.parent = m.transform;
                    _calderosApagados[k-1].gameObject.SetActive(true);
                    //_caldero.Restart();
                    _calderosApagados[k-1].transform.position = GetPosition(m) + Vector3.up * 1.75f;
                }
                k++;
            }
            

            // Set caldero loco
            _caldero.transform.parent = meshes[0, 0, 0].transform;
            _caldero.gameObject.SetActive(true);
            _caldero.Restart();
            _caldero.transform.position = GetInitialPosition() + Vector3.up * 1.75f;

            OnWorldReady.Invoke();

            first = false;
        }

        void GetRandomEnvironments(out Environments e1, out Environments e2)
        {
            int ie1 = Random.Range(0, 3);
            int ie2 = Random.Range(0, 3);
            if (ie1 == ie2) ie2 = ++ie2 % 3;

            e1 = (Environments)ie1;
            e2 = (Environments)ie2;
        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab)) {
                _generateChunkLoop.ReGenerate();
            }
        }
    
    }

}
