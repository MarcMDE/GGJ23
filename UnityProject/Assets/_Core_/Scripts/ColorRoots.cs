using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGJ23;

public class ColorRoots : MonoBehaviour
{
    [SerializeField]
    float _boundsBlendRange = 0.1f;
    [SerializeField]
    float _blendRange = 0.2f;
    const int _nEnvironments = 4;
    Color[] _colors = new Color[_nEnvironments];

    void Start()
    {
        _colors[(int)Environments.AUTUM] = Color.red;
        _colors[(int)Environments.WINTER] = Color.white;
        _colors[(int)Environments.SPRING] = Color.green;
        _colors[(int)Environments.NONE] = Color.black;
    }

    private float Scale(float value, float min, float max, float minScale, float maxScale)
    {
        float scaled = minScale + (value - min) / (max - min) * (maxScale - minScale);
        return scaled;
    }

    public void Paint(float f1, float f2, Environments e1, Environments e2, GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Color[] colors = new Color[vertices.Length];
        //float[] values = new float[vertices.Length];

        float max = float.MinValue;
        float min = float.MaxValue;

        int axis = Random.Range(0, 2);
        if (axis == 1) axis = 2;

        for (int i = 0; i < vertices.Length; i++)
        {
            float v = vertices[i][axis];
            if (v < min) min = v;
            if (v > max) max = v;
        }

        float range = 1 - (_boundsBlendRange * 2 + _blendRange);

        for (int i = 0; i < vertices.Length; i++)
        {
            float f = Scale(vertices[i][axis], min, max, 0, 1);

            if (f < _boundsBlendRange)
            {
                float blendF = Scale(f, 0, _boundsBlendRange, 0, 1);
                colors[i] = Color.Lerp(_colors[(int)Environments.NONE], _colors[(int)e1], blendF);
            }
            else if (f > 1-_boundsBlendRange)
            {
                float blendF = Scale(f, 1-_boundsBlendRange, 1, 0, 1);
                colors[i] = Color.Lerp(_colors[(int)e2], _colors[(int)Environments.NONE], blendF);
            }
            else if (f < _boundsBlendRange + f1 * range)
            {
                colors[i] = _colors[(int)e1];
            }
            else if (f < _boundsBlendRange + f1 * range + _blendRange)
            {
                float blendF = Scale(f, _boundsBlendRange + f1 * range, _boundsBlendRange + f1 * range + _blendRange, 0, 1);
                colors[i] = Color.Lerp(_colors[(int)e1], _colors[(int)e2], blendF);
            }
            else
            {
                colors[i] = _colors[(int)e2];
            }

        }

        if (axis == 0) axis = 2;
        else axis = 0;

        // Alt axis
        for (int i = 0; i < vertices.Length; i++)
        {
            float f = Scale(vertices[i][axis], min, max, 0, 1);

            if (f < _boundsBlendRange)
            {
                float blendF = Scale(f, 0, _boundsBlendRange, 0, 1);
                colors[i] = Color.Lerp(_colors[(int)Environments.NONE], colors[i], blendF);
            }
            else if (f > 1 - _boundsBlendRange)
            {
                float blendF = Scale(f, 1 - _boundsBlendRange, 1, 0, 1);
                colors[i] = Color.Lerp(colors[i], _colors[(int)Environments.NONE], blendF);
            }
        }

            // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
}
