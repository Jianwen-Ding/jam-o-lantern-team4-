using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handMover : MonoBehaviour
{
    [SerializeField]
    GameObject orbitingObject;
    playerCam getAngle;
    [SerializeField]
    float distanceFromCenter;
    /*[SerializeField]
    float xTest;
    [SerializeField]
    float yTest;*/  
    // Start is called before the first frame update
    void Start()
    {
        getAngle = Camera.main.GetComponent<playerCam>();
    }

    // Update is called once per frame
    void Update()
    {
        float actualHorRot = getAngle.yAngle;
        float actualVerRot = -getAngle.xAngle;
        //float actualHorRot = yTest;
        //float actualVerRot = -xTest;
        float verDist = distanceFromCenter * Mathf.Sin(actualVerRot * Mathf.Deg2Rad);
        float horDist = distanceFromCenter * Mathf.Cos(actualVerRot * Mathf.Deg2Rad);
        gameObject.transform.position = new Vector3(horDist * Mathf.Sin(actualHorRot * Mathf.Deg2Rad) + orbitingObject.gameObject.transform.position.x, verDist + orbitingObject.gameObject.transform.position.y, horDist * Mathf.Cos(actualHorRot * Mathf.Deg2Rad) + orbitingObject.gameObject.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(-actualVerRot, actualHorRot, 0);
    }
}
