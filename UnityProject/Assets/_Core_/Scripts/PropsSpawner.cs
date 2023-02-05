using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{
    const float _dotThreshold = 0.35f;
    void Start()
    {
        //Spawn();
    }

    public void Spawn(GameObject ground, int n, GameObject prefab)
    {
        Mesh groundMesh = ground.GetComponent<MeshFilter>().mesh;

        for (int i = 0; i < n; i++)
        {
            int vIndex;
            Vector3 spawnPos;
            Vector3 spawnNormal;
            // TODO: Avoid repetitions
            do
            {
                vIndex = Random.Range(0, groundMesh.vertices.Length);
                spawnPos = groundMesh.vertices[vIndex] + ground.transform.position;
                spawnNormal = groundMesh.normals[vIndex];
            } while (Vector3.Dot(spawnNormal, Vector3.up) < _dotThreshold);

            Instantiate(prefab, spawnPos, Quaternion.LookRotation(Vector3.Cross(spawnNormal, Vector3.right), spawnNormal), ground.transform);
        }
    }


    void Update()
    {
        
    }
}
