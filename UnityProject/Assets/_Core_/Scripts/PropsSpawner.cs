using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{
    [SerializeField] GameObject _collectablePrefab;
    [SerializeField] GameObject _ground;
    [SerializeField] int _n;

    Vector3[] _spawnIndices;

    void Start()
    {
        //Spawn();
    }

    public void SetGround(GameObject ground)
    {
        _ground = ground;
    }

    public void Spawn()
    {
        Mesh groundMesh = _ground.GetComponent<MeshFilter>().mesh;
        _spawnIndices = new Vector3[_n];

        for (int i = 0; i < _n; i++)
        {
            int vIndex;
            Vector3 spawnPos;
            Vector3 spawnNormal;
            // TODO: Avoid repetitions
            do
            {
                vIndex = Random.Range(0, groundMesh.vertices.Length);
                spawnPos = groundMesh.vertices[vIndex];
                spawnNormal = groundMesh.normals[vIndex];
            } while (Vector3.Dot(spawnNormal, Vector3.up) < 0.1f);

            Instantiate(_collectablePrefab, spawnPos, Quaternion.LookRotation(Vector3.Cross(spawnNormal, Vector3.right), spawnNormal), transform);
        }
    }


    void Update()
    {
        
    }
}
