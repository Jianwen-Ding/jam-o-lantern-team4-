using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mathHelper : MonoBehaviour
{
    public static float distance(float x, float y)
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public static float distance(Vector3 vec1, Vector3 vec2)
    {
        float xDiff = vec1.x - vec2.x;
        float yDiff = vec1.y - vec2.y;
        float zDiff = vec1.z - vec2.z;
        return Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);
    }

    public static Vector3 getVectorFromAngle(float magnitude, Vector2 angles)
    {
        float actualHorRot = angles.y;
        float actualVerRot = -angles.x;
        float verDist = magnitude * Mathf.Sin(actualVerRot * Mathf.Deg2Rad);
        float horDist = magnitude * Mathf.Cos(actualVerRot * Mathf.Deg2Rad);
        return new Vector3(horDist * Mathf.Sin(actualHorRot * Mathf.Deg2Rad), verDist, horDist * Mathf.Cos(actualHorRot * Mathf.Deg2Rad));
    }

    public static Vector2 getAngleBetweenVec(Vector3 vec1, Vector3 vec2)
    {
        float xDiff = vec1.x - vec2.x;
        float yDiff = vec1.y - vec2.y;
        float zDiff = vec1.z - vec2.z;
        float horAngle = Mathf.Atan2(xDiff, zDiff) * Mathf.Rad2Deg;
        float verAngle = Mathf.Atan2(yDiff, distance(xDiff,zDiff)) * Mathf.Rad2Deg;
        return new Vector2(verAngle, horAngle - 180);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
