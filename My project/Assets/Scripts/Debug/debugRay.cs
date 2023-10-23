using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugRay : MonoBehaviour
{
    public static void createRay(Vector3 position, Vector3 direction, float distance, int layerMask)
    {
        RaycastHit hit;
        if(Physics.Raycast(position, direction, out hit, distance, layerMask))
        {
            Debug.DrawLine(position, hit.point, Color.magenta, 0.1f);
        }
        else
        {
            Debug.DrawRay(position, direction, Color.magenta, 0.1f, true);
        }
    }
}
