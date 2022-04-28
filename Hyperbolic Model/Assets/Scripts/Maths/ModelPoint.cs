using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPoint
{
    public Quaternion x1;
    public Quaternion x2;

    public Complex idealBase;

    public ModelPoint(Quaternion pos)
    {
        x1 = pos;
        x2 = 1;

        idealBase = new Complex(pos.x, pos.y);
    }

    // CONVERT
    public static implicit operator ModelPoint(Quaternion p)
    {
        return new ModelPoint(p);
    }

    // CONVERT
    public static implicit operator ModelPoint(Complex z)
    {
        return new ModelPoint(z);
    }

    // CONVERT
    public static implicit operator ModelPoint(float s)
    {
        return new ModelPoint(s);
    }

    public Vector3 AsVector()
    {
        return x1.AsVector();
    }

    public Vector3 Unity_AsVector()
    {
        return new Vector3(x1.x, x1.z, x1.y);
    }

    public void ActByMob(Matrix mat)
    {
        if(mat.Determinant() == 1)
        {
            var t1 = mat.a * x1 + mat.b;
            var t2 = mat.c * x1 + mat.b;
            x1 = t1;
            x2 = t2;

            Normalise();
        }
    }

    private void Normalise()
    {
        if(x2 == 0)
        {
            x1 = -Quaternion.j;
            idealBase = 0;
        }
        else
        {
            x1 = x1 /  x2;
            x2 = 1;

            idealBase = new Complex(x1.x, x1.y);
        }
    }

    public bool IsAtInfty()
    {
        return (x2 == 0);
    }

    public bool IsIdeal()
    {
        return (x1.z == 0);
    }

    public Quaternion AsQuaternion()
    {
        return x1;
    }
}
