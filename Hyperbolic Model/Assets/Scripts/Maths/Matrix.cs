using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix 
{
    public Complex a;
    public Complex b;
    public Complex c;
    public Complex d;

    public Matrix(Complex in_a, Complex in_b, Complex in_c, Complex in_d)
    {
        a = in_a;
        b = in_b;
        c = in_c;
        d = in_d;
    }

    public static Matrix identity = new Matrix(1, 0, 0, 1);

    // EQUALITY
    public static bool operator ==(Matrix matA, Matrix matB)
    {
        return ((matA.a == matB.a) && (matA.b == matB.b) && (matA.c == matB.c) && (matA.d == matB.d));
    }

    // EQUALITY
    public static bool operator !=(Matrix matA, Matrix matB)
    {
        return !(matA == matB);
    }

    // EQUALITY
    public override bool Equals(object matB)
    {
        var matB2 = (Matrix)matB;
        return (this == matB2);
    }

    // EQUALITY
    public override int GetHashCode()
    {
        return unchecked(((a.GetHashCode() * 17 + b.GetHashCode()) * 17 + c.GetHashCode()) * 17 + d.GetHashCode());
    }

    // DETERMINANT
    public Complex Determinant()
    {
        return a * d - b * c;
    }

    // INVERSE
    public Matrix Inverse()
    {
        if(Determinant() == 0)
        {
            throw new ArgumentException(String.Format("Cannot invert a singular matrix"));
        }
        else
        {
            return new Matrix(d, -b, -c, a) * (1 / Determinant());
        }
    }

    // TRACE
    public Complex Trace()
    {
        return a + d;
    }

    // MOBIUS
    public static Quaternion operator &(Matrix matA, Quaternion p)
    {
        return (matA.a * p + matA.b) / (matA.c * p + matA.d);
    }

    // SCALING
    public static Matrix operator *(Matrix matA, Complex z)
    {
        return new Matrix(matA.a * z, matA.b * z, matA.c * z, matA.d * z);
    }

    // SCALING
    public static Matrix operator *(Complex z, Matrix matA)
    {
        return matA * z;
    }

    // ADDITION
    public static Matrix operator +(Matrix matA, Matrix matB)
    {
        return new Matrix(matA.a + matB.a, matA.b + matB.b, matA.c + matB.c, matA.d + matB.d);
    }

    // SUBTRACTION
    public static Matrix operator -(Matrix matA)
    {
        return (-1) * matA;
    }

    // SUBTRACTION
    public static Matrix operator -(Matrix matA, Matrix matB)
    {
        return matA + -matB;
    }

    //MULTIPLICATION
    public static Matrix operator *(Matrix matA, Matrix matB)
    {
        return new Matrix(matA.a * matB.a + matA.b * matB.c,
                          matA.a * matB.b + matA.b * matB.d,
                          matA.c * matB.a + matA.d * matB.c,
                          matA.c * matB.b + matA.d * matB.d);
    }
}
