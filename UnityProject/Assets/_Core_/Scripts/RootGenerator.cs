using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CUBE_ORIENTATION { XP=0, XN, ZP, ZN};

public class Line
{
    Vector3 _start, _end;
    float? _magnitude;
    Vector3? _direction;

    public Line(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
        _magnitude = null;
        _direction = null;
    }

    public float Magnitude 
    { 
        get 
        {
            if (_magnitude is null)
            {
                _magnitude = Vector3.Magnitude(_end - _start);
            }
            
            return _magnitude.Value; 
        } 
    }

    public Vector3 Direction
    {
        get
        {
            if (_direction is null)
            {
                _direction = (_end - _start) / Magnitude;
            }

            return _direction.Value;
        }
    }

    public Vector3 GetRandomPoint()
    {
        float r = Random.Range(0, 1) * Magnitude;
        Vector3 point = _start + Direction * r;

        return point;
    }

    public void Draw(float scale)
    {
        Debug.DrawLine(_start * scale, _end * scale);
    }

    public float GetDistance(Vector3 p)
    {
        return Vector3.Cross(Direction, p - _start).magnitude;
    }

}

public class RootGenerator : MonoBehaviour
{
    const int _securityOffset = 2;

    [SerializeField]
    float _std = 2;

    [SerializeField]
    float _worldScale = 0.1f;

    [SerializeField]
    int _matrixSize = 16;

    [SerializeField]
    int _targetPopulation = 10;

    float[,,] _matrix;

    List<Line> _lines;

    int _range;

    void Start()
    {
        _lines = new List<Line>();
        _matrix = new float[_matrixSize, _matrixSize, _matrixSize];
        _range = _matrixSize - _securityOffset * 2;

        Vector3[] _startingPoints = new Vector3[4];

        Vector3 midPoint = Vector3.one * (_matrixSize / 2);
        Vector2 point = GetRandomPoint();
        _startingPoints[(int)CUBE_ORIENTATION.XN] = new Vector3(0, point.y, point.x);
        point = GetRandomPoint();
        _startingPoints[(int)CUBE_ORIENTATION.XP] = new Vector3(_matrixSize-1, point.y, point.x);
        point = GetRandomPoint();
        _startingPoints[(int)CUBE_ORIENTATION.ZN] = new Vector3(point.x, point.y, 0);
        point = GetRandomPoint();
        _startingPoints[(int)CUBE_ORIENTATION.ZP] = new Vector3(point.x, point.y, _matrixSize-1);



        for (int i=0; i<_startingPoints.Length; i++)
        {
            _lines.Add(new Line(_startingPoints[i], _startingPoints[Random.Range(0, _startingPoints.Length)]));
        }

        for (int i=0; i<2; i++)
        {
            _lines.Add(new Line(midPoint, _startingPoints[Random.Range(0, _startingPoints.Length)]));
        }

        ComputeMatrix();
    }

    void ComputeMatrix()
    {
        for (int z=0; z<_matrixSize; z++)
        {
            for (int y = 0; y < _matrixSize; y++)
            {
                for (int x = 0; x < _matrixSize; x++)
                {
                    float minDist = float.MaxValue;
                    foreach (var l in _lines)
                    {
                        float lineDist = l.GetDistance(new Vector3(x, y, z));
                        if (lineDist < minDist) minDist = lineDist;
                    }
                    //_matrix[x, y, z] = minDist;
                    _matrix[x, y, z] = Gaussian1D(minDist);

                    //Debug.Log($"Dist: {minDist} - Weigth: {Gaussian1D(minDist)}");
                }
            }
        }
    }

    float Gaussian1D(float d)
    {
        float weight = 1 / (Mathf.Sqrt(2 * Mathf.PI) * Mathf.Exp(Mathf.Pow(d, 2) / (2 * Mathf.Pow(_std, 2))));
        return weight;
    }


    Vector2 GetRandomPoint()
    {
        float x = Random.Range(_securityOffset, _securityOffset + _range);
        float y = Random.Range(_securityOffset, _securityOffset + _range);

        return new Vector2(x, y);
    }

    void Update()
    {
        foreach (var l in _lines)
        {
            l.Draw(_worldScale);
        }
    }
}
