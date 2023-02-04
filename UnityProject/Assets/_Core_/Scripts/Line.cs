using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ23{
    public class Line
    {
        Vector3 _start, _end;
        float? _magnitude;
        Vector3? _direction;

        public Vector3 Start { get { return _start; } }
        public Vector3 End { get { return _end; } }
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

        public Vector3 GetPoint(float f)
        {
            return _start + Direction * Magnitude * f;
        }

        public void Draw(float scale)
        {
            Debug.DrawLine(_start * scale, _end * scale, Random.ColorHSV());
        }

        public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 relativePoint = point - lineStart;
            Vector3 lineDirection = lineEnd - lineStart;
            float length = lineDirection.magnitude;
            Vector3 normalizedLineDirection = lineDirection;
            if (length > .000001f)
                normalizedLineDirection /= length;

            float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
            dot = Mathf.Clamp(dot, 0.0F, length);

            return lineStart + normalizedLineDirection * dot;
        }

        // Calculate distance between a point and a line.
        public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
        }

        public float GetDistance(Vector3 p)
        {
            //return Vector3.Cross(Direction, p - _start).magnitude;
            return DistancePointLine(p, _start, _end);
        }

    }

}
