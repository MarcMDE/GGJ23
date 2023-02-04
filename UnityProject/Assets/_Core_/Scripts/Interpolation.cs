using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Interpolation
{
    public static float Interpolate(float y0, float y1, float y2, float y3, float mu)
    {
        float a0, a1, a2, a3, mu2;

        mu2 = mu * mu;
        a0 = y3 - y2 - y0 + y1;
        a1 = y0 - y1 - a0;
        a2 = y2 - y0;
        a3 = y1;

        return (a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3);
    }

    public static Vector3 Interpolate3D(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float mu)
    {
        return new Vector3(Interpolate(p1.x, p2.x, p3.x, p4.x, mu), Interpolate(p1.x, p2.x, p3.x, p4.x, mu), Interpolate(p1.x, p2.x, p3.x, p4.x, mu));
    }
}
