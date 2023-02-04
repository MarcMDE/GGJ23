using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CUBE_ORIENTATION { XP=0, XN, ZP, ZN};
namespace GGJ23{
    public class RootGenerator : MonoBehaviour
    {
        [SerializeField] int _rootSmoothIterations = 4;

        [SerializeField] float _randomness = 0.5f;

        [SerializeField]
        int _securityOffset = 2;

        [SerializeField]
        float _std = 2;

        [SerializeField]
        float _worldScale = 0.1f;

        [SerializeField]
        int _matrixSize = 16;

        [SerializeField]
        int _targetPopulation = 10;

        [SerializeField]
        int numStartPoints = 2;
        float[,,] _matrix;

        List<Line> _lines;
        List<Line> _connectionLines;

        int _range;

        MeshGenerator _meshGenerator;

        async void Start()
        {
            _meshGenerator = GetComponent<MeshGenerator>();

            _lines = new List<Line>();
            _connectionLines = new List<Line>();

            _matrix = new float[_matrixSize, _matrixSize, _matrixSize];
            _range = _matrixSize - _securityOffset * 2;

            Vector3[,] _startingPoints = new Vector3[4,numStartPoints];

            Vector3 midPoint = Vector3.one * (_matrixSize / 2);
            Vector2 point;
            
            int sides = _startingPoints.GetLength(0);

            for(int i = 0; i < numStartPoints; i++){
                point = GetRandomPoint();
                _startingPoints[(int)CUBE_ORIENTATION.XN,i] = new Vector3(_securityOffset, point.y, point.x);
                _connectionLines.Add( new Line(new Vector3(0,point.y,point.x), new Vector3(_securityOffset,point.y,point.x) ));
            }
            for(int i = 0; i < numStartPoints; i++){
                point = GetRandomPoint();
                _startingPoints[(int)CUBE_ORIENTATION.XP,i] = new Vector3(_matrixSize-1-_securityOffset, point.y, point.x);
                _connectionLines.Add( new Line(new Vector3(_matrixSize-1-_securityOffset, point.y, point.x), new Vector3(_matrixSize-1, point.y, point.x) ));
            }
            for(int i = 0; i < numStartPoints; i++){
                point = GetRandomPoint();
                _startingPoints[(int)CUBE_ORIENTATION.ZN,i] = new Vector3(point.x, point.y, _securityOffset);
                _connectionLines.Add( new Line(new Vector3(point.x, point.y, 0), new Vector3(point.x, point.y, _securityOffset)) );
            }
            for(int i = 0; i < numStartPoints; i++){
                point = GetRandomPoint(); 
                _startingPoints[(int)CUBE_ORIENTATION.ZP,i] = new Vector3(point.x, point.y, _matrixSize-1-_securityOffset);
                _connectionLines.Add( new Line(new Vector3(point.x, point.y, _matrixSize-1-_securityOffset), new Vector3(point.x, point.y, _matrixSize-1)) );
            }
            
            for (int i=0; i<sides; i++)
            {
                for (int j=0; j<numStartPoints; j++)
                {
                    int r,r2;
                    Vector3 currentPoint = _startingPoints[i,j];
                    do
                    {
                        r = Random.Range(0, sides);
                        r2 = Random.Range(0, numStartPoints);
                    } while(currentPoint == _startingPoints[r,r2]);

                    Line l = new Line(_startingPoints[i, j], _startingPoints[r, r2]);
                    //_lines.Add(l);
                    Vector3[] points = new Vector3[_rootSmoothIterations + 1];
                    points[0] = l.Start;
                    points[points.Length - 1] = l.End;
                    Vector3 prevPoint = l.Start;
                    for (int k = 1; k < _rootSmoothIterations; k++)
                    {
                        points[k] = l.GetPoint(((float)k) / _rootSmoothIterations) + new Vector3(
                            Random.Range(-_randomness, _randomness),
                            Random.Range(-_randomness, _randomness),
                            Random.Range(-_randomness, _randomness));

                        _lines.Add(new Line(prevPoint, points[k]));
                        prevPoint = points[k];
                    }

                    _lines.Add(new Line(prevPoint, l.End));

                }
            }

            List<Vector3> usedPoints = new List<Vector3>();
            for (int i=0; i<2; i++)
            {
                int r, r2;
                do{
                    r = Random.Range(0, sides);
                    r2 = Random.Range(0, numStartPoints);
                }while(usedPoints.Contains(_startingPoints[r,r2]));

                // Define interpolations
                Line l = new Line(midPoint, _startingPoints[r, r2]);
                Vector3 [] points = new Vector3[_rootSmoothIterations+1];
                points[0] = l.Start;
                points[points.Length - 1] = l.End;
                Vector3 prevPoint = l.Start;
                for (int j=1; j<_rootSmoothIterations; j++)
                {
                    points[j] = l.GetPoint(((float)j) / _rootSmoothIterations) + new Vector3(
                        Random.Range(-_randomness, _randomness), 
                        Random.Range(-_randomness, _randomness), 
                        Random.Range(-_randomness, _randomness));

                    _lines.Add(new Line(prevPoint, points[j]));
                    prevPoint = points[j];
                }

                _lines.Add(new Line(prevPoint, l.End));


                usedPoints.Add(_startingPoints[r,r2]);
            }
            //AddDensity(_startingPoints);

            ComputeMatrix();

            _meshGenerator.LoadData(_matrix);
        }

        void AddDensity(Vector3[] points)
        {
            foreach (var l in _lines)
            {
                Vector3 newPoint = l.GetRandomPoint();
                _lines.Add(new Line(newPoint, points[Random.Range(0, points.Length)]));
            }
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
                        foreach (var l in _connectionLines)
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
            /*
            foreach (var l in _lines)
            {
                l.Draw(_worldScale);
            }
            */
        }
    }

}
