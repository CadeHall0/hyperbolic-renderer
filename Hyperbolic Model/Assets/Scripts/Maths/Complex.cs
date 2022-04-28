using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex
{
    public float x;
    public float y;

    public Complex(float re, float im)
    {
        x = re;
        y = im;
    }

    public static Complex i = new Complex(0, 1);

    // EQUALITY
    public static bool operator ==(Complex z, Complex w)
    {
        return ((z.x == w.x) && (z.y == w.y));
    }

    // EQUALITY
    public static bool operator !=(Complex z, Complex w)
    {
        return !(z == w);
    }

    // EQUALITY
    public override bool Equals(object w)
    {
        var w2 = (Complex)w;
        return (this == w2);
    }

    // EQUALITY
    public override int GetHashCode()
    {
        return unchecked(x.GetHashCode() * 17 + y.GetHashCode());
    }

    // CONVERT
    public static implicit operator Complex(float num)
    {
        return new Complex(num, 0);
    }

    // DISPLAY
    public override string ToString()
    {
        return x.ToString() + " + " + y.ToString() + "i";
    }

    // CONJUGATE
    public Complex Conjugate()
    {
        return new Complex(x, -y);
    }

    // MAGNITUDE
    public float Magnitude()
    {
        return Mathf.Pow(x * x + y * y, 0.5f);
    }

    // SQR MAGNITUDE
    public float SQRMagnitude()
    {
        return x * x + y * y;
    }

    // INVERSE
    public Complex Inverse()
    {
        return Conjugate() * (1 / SQRMagnitude());
    }

    // ADDITION
    public static Complex operator+ (Complex z, Complex w)
    {
        return new Complex(z.x + w.x, z.y + w.y);
    }

    // SUBTRACTION
    public static Complex operator- (Complex z, Complex w)
    {
        return z + (-1*w);
    }

    //SUBTRACTION
    public static Complex operator -(Complex z)
    {
        return (-1)*z;
    }

    // MULTIPLICATION
    public static Complex operator* (Complex z, Complex w)
    {
        return new Complex(z.x * w.x - z.y * w.y,
                           z.x * w.y + z.y * w.x);
    }

    // DIVISION
    public static Complex operator/ (Complex z, Complex w)
    {
        return z * w.Inverse();
    }
}
