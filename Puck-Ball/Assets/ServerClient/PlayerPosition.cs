using System;
using UnityEngine;

[Serializable]
public class PlayerPosition
{
    public float X;
    public float Y;
    public float Z;

    public Vector3 ToVector()
    {
        return new Vector3(X, Y, Z);
    }

    public static PlayerPosition FromVector(Vector3 position)
    {
        return new PlayerPosition
        {
            X = position.x,
            Y = position.y,
            Z = position.z,
        };
    }
}
