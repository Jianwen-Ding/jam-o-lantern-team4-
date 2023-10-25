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
        debugValueSys.display("angle", " angle: " + (int)getAngle.xAngle + "," + (int)getAngle.yAngle);
        Vector2 check = mathHelper.getAngleBetweenVec(orbitingObject.transform.position, gameObject.transform.position);
        debugValueSys.display("get angle", " recieved angle: " + (int)check.x + "," + (int)check.y);
        Vector3 addVec = mathHelper.getVectorFromAngle(distanceFromCenter, getAngle.getAngleVec());
        gameObject.transform.position = orbitingObject.transform.position + addVec;
        gameObject.transform.rotation = Quaternion.Euler(getAngle.xAngle , getAngle.yAngle, 0);
    }
}
