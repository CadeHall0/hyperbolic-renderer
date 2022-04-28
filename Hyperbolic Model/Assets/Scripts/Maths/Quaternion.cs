using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion(float re, float i, float j, float k)
    {
        x = re;
        y = i;
        z = j;
        w = k;
    }

    public static Quaternion i = new Quaternion(0, 1, 0, 0);
    public static Quaternion j = new Quaternion(0, 0, 1, 0);
    public static Quaternion k = new Quaternion(0, 0, 0, 1);

    // VECTOR
    public Vector3 AsVector()
    {
        return new Vector3(x, y, z);
    }

    // EQUALITY
    public static bool operator ==(Quaternion p, Quaternion q)
    {
        return ((p.x == q.x) && (p.y == q.y) && (p.z == q.z) && (p.w == q.w));
    }

    // EQUALITY
    public static bool operator !=(Quaternion p, Quaternion q)
    {
        return !(p == q);
    }

    // EQUALITY
    public override bool Equals(object q)
    {
        var q2 = (Quaternion)q;
        return (this == q2);
    }

    // EQUALITY
    public override int GetHashCode()
    {
        return unchecked(((x.GetHashCode() * 17 + y.GetHashCode()) * 17 + z.GetHashCode())*17 + w.GetHashCode()) ;
    }

    // CONVERT
    public static implicit operator Quaternion(float num)
    {
        return new Quaternion(num, 0, 0, 0);
    }

    // CONVERT
    public static implicit operator Quaternion(Complex c)
    {
        return new Quaternion(c.x, c.y, 0, 0);
    }

    // DISPLAY
    override public string ToString()
    {
        return x.ToString() + " + " + y.ToString() + "i + " + z.ToString() + "j + " + w.ToString() + "k";
    }

    // CONJUGATION
   public Quaternion Conjugate()
    {
        return new Quaternion(x, -y, -z, -w);
    }

    // INVERSE
    public Quaternion Inverse()
    {
        return Conjugate() * (1/SQRMagnitude());
    }

    // MAGNITUDE
    public float Magnitude()
    {
        return Mathf.Pow(x*x + y*y + z*z + w*w, 0.5f) ;
    }

    // SQR MAGNITUDE
    public float SQRMagnitude()
    {
        return x * x + y * y + z * z + w * w;
    }

    // ADDITION
    public static Quaternion operator +(Quaternion p, Quaternion q)
    {
        Quaternion result = new Quaternion(p.x + q.x, 
                                           p.y + q.y, 
                                           p.z + q.z, 
                                           p.w + q.w);
        return result;
    }

    // SUBTRACTION
    public static Quaternion operator -(Quaternion p, Quaternion q)
    {
        return p + (-1 * q);
    }

    // SUBTRACTION
    public static Quaternion operator -(Quaternion p)
    {
        return (-1 * p);
    }

    // MULTIPLICATION
    public static Quaternion operator *(Quaternion p, Quaternion q)
    {
        Quaternion result = new Quaternion(p.x * q.x - p.y * q.y - p.z * q.z - p.w * q.w,
                                           p.x * q.y + p.y * q.x + p.z * q.w - p.w * q.z,
                                           p.x * q.z + p.z * q.x + p.w * q.y - p.y * q.w,
                                           p.x * q.w + p.w * q.x + p.y * q.z - p.z * q.y);
        return result;
    }

    // DIVISION
    public static Quaternion operator /(Quaternion p, Quaternion q)
    {
        return p * q.Inverse();
    }
}
