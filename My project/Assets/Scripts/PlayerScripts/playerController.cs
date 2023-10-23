using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Cache
    [SerializeField]
    GameObject cameraHolder;
    [SerializeField]
    playerCam getCam;
    [SerializeField]
    Rigidbody objectPhysics;
    //Movement
    [SerializeField]
    float groundSpeed;
    [SerializeField]
    float airSpeed;
    [SerializeField]
    float jumpForce;
    //Checks whether midair or not
    [SerializeField]
    bool midAir;
    [SerializeField]
    float distanceHorPerCheck;
    [SerializeField]
    float distanceVerPerCheck;
    // Start is called before the first frame update
    void Start()
    {
        getCam = Camera.main.GetComponent<playerCam>();
        objectPhysics = gameObject.GetComponent<Rigidbody>();
    }

    private bool checkBeneath()
    {
        int layerMask = ~(1 << 6);
        bool centerCheck = Physics.Raycast(gameObject.transform.position, Vector3.down, distanceVerPerCheck, layerMask);
        debugRay.createRay(gameObject.transform.position, Vector3.down, distanceVerPerCheck, layerMask);
        float initialAngle = -gameObject.transform.eulerAngles.y;
        bool checkAtAngle(float angle)
        {
            float usedAngle = initialAngle + angle;
            Vector3 aug = new Vector3(distanceHorPerCheck * Mathf.Cos(usedAngle * Mathf.Deg2Rad), 0, distanceHorPerCheck * Mathf.Sin(usedAngle * Mathf.Deg2Rad));
            debugRay.createRay(gameObject.transform.position + aug, Vector3.down, distanceVerPerCheck, layerMask);
            debugValueSys.display("" + angle, "" + usedAngle);  
            return Physics.Raycast(gameObject.transform.position + aug, Vector3.down, distanceVerPerCheck, layerMask);
        }
        bool leftTopCheck = checkAtAngle(45f);
        bool rightTopCheck = checkAtAngle(-45f);
        bool leftBackCheck = checkAtAngle(135f);
        bool rightBackCheck = checkAtAngle(-135f);
        return (centerCheck || leftTopCheck || rightTopCheck || leftBackCheck || rightBackCheck);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (checkBeneath())
        {
            midAir = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (checkBeneath())
        {
            midAir = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        midAir = true;
    }
    // Update is called once per frame
    void Update()
    {
        cameraHolder.transform.position = gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0, getCam.yAngle, 0);
        float verInput = Input.GetAxisRaw("Verticle");
        float horInput = Input.GetAxisRaw("Horizontal");
        //walking and strafing midair
        if (verInput != 0 || horInput != 0)
        {
            float yAngleAdjust = 0;
            if (verInput > 0 && horInput == 0)
            {
                yAngleAdjust = 90;
            }
            else if (verInput > 0 && horInput > 0)
            {
                yAngleAdjust = 45;
            }
            else if (verInput == 0 && horInput > 0)
            {
                yAngleAdjust = 0;
            }
            else if (verInput < 0 && horInput > 0)
            {
                yAngleAdjust = -45;
            }
            else if (verInput < 0 && horInput == 0)
            {
                yAngleAdjust = -90;
            }
            else if (verInput < 0 && horInput < 0)
            {
                yAngleAdjust = -135;
            }
            else if (verInput == 0 && horInput < 0)
            {
                yAngleAdjust = -180;
            }
            else if (verInput > 0 && horInput < 0)
            {
                yAngleAdjust = 135;
            }
            float moveDir = yAngleAdjust - gameObject.transform.eulerAngles.y;
            //Strafing midair
            if (midAir)
            {
                Vector3 moveAccel = new Vector3(airSpeed * Mathf.Cos(moveDir * Mathf.Deg2Rad), 0, airSpeed * Mathf.Sin(moveDir * Mathf.Deg2Rad));
                objectPhysics.AddForce(moveAccel * Time.deltaTime, ForceMode.Acceleration);
            }
            //Walking on ground
            else
            {
                Vector3 moveVel = new Vector3(groundSpeed * Mathf.Cos(moveDir * Mathf.Deg2Rad), objectPhysics.velocity.y, groundSpeed * Mathf.Sin(moveDir * Mathf.Deg2Rad));
                objectPhysics.velocity = moveVel;
            }

        }

        float spaInput = Input.GetAxisRaw("Space");
        //jumping
        if (spaInput != 0)
        {
            if (midAir)
            {
                objectPhysics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
