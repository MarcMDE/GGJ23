using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorRoots : MonoBehaviour
{
    const int _nEnvironments = 4;
    Color[] _colors = new Color[_nEnvironments];

    [SerializeField]
    GameObject _object;

    void Start()
    {
        _colors[(int)Environments.AUTUM] = Color.red;
        _colors[(int)Environments.WINTER] = Color.white;
        _colors[(int)Environments.SPRING] = Color.green;
        _colors[(int)Environments.NONE] = Color.black;

        Mesh mesh = _object.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }

    void Update()
    {
        
    }
}
