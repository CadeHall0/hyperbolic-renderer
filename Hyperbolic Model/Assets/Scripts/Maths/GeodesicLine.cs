using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeodesicLine
{
    public bool isEucOrth;

    public ModelPoint start;
    public ModelPoint end;

    public Complex idealCenter;

    public GeodesicLine(ModelPoint a, ModelPoint b)
    {
        start = a;
        end = b;

        Normalise();
    }

    public void ActByMob(Matrix mat)
    {
        if(mat.Determinant() == 1)
        {
            start.ActByMob(mat);
            end.ActByMob(mat);

            Normalise();
        }
    }

    public void Normalise()
    {
        if (start.IsAtInfty() || end.IsAtInfty() || start.idealBase == end.idealBase)
        {
            isEucOrth = true;
            idealCenter = 0;
        }
        else
        {
            FindIdealCenter();
        }
    }

    private Vector3 dir;
    private Vector3 perp;
    private Vector3 planeNormal;
    private void FindIdealCenter()
    {
        if(start.x1.z == 0 && end.x1.z == 0)
        {
            idealCenter = (start.idealBase + end.idealBase) / 2;
        }
        else
        {
            var endVec = end.AsVector();
            var startVec = start.AsVector();

            planeNormal = Vector3.Cross(startVec, endVec);
            dir = (endVec - startVec)/2;
            perp = Vector3.Cross(planeNormal, dir);

            var t = -(startVec.z + dir.z) / (perp.z);
            idealCenter.x = startVec.x + dir.x + t*perp.x;
            idealCenter.y = startVec.y + dir.y + t * perp.y;
        }
    }
}
