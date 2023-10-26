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
    [SerializeField]
    bool isPlayedObject;
    public Vector2 givenAngle;
    /*[SerializeField]
    float xTest;
    [SerializeField]
    float yTest;*/
    // Start is called before the first frame update
    void Start()
    {
        getAngle = Camera.main.GetComponent<playerCam>();
        givenAngle = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayedObject)
        {
            givenAngle = getAngle.getAngleVec();
        }
        //debugValueSys.display("angle", " angle: " + (int)getAngle.xAngle + "," + (int)getAngle.yAngle);
        Vector2 check = mathHelper.getAngleBetweenVec(orbitingObject.transform.position, gameObject.transform.position);
        //debugValueSys.display("get angle", " recieved angle: " + (int)check.x + "," + (int)check.y);
        Vector3 addVec = mathHelper.getVectorFromAngle(distanceFromCenter, givenAngle);
        gameObject.transform.position = orbitingObject.transform.position + addVec;
        gameObject.transform.rotation = Quaternion.Euler(givenAngle.x , givenAngle.y, 0);
    }
}
