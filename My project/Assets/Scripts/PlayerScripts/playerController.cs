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
    float midairReversalRatio;
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
            //debugValueSys.display("" + angle, "" + usedAngle);  
            return Physics.Raycast(gameObject.transform.position + aug, Vector3.down, distanceVerPerCheck, layerMask);
        }
        bool leftTopCheck = checkAtAngle(45f);
        bool rightTopCheck = checkAtAngle(-45f);
        bool leftBackCheck = checkAtAngle(135f);
        bool rightBackCheck = checkAtAngle(-135f);
        return (centerCheck || leftTopCheck || rightTopCheck || leftBackCheck || rightBackCheck);
    }
    private void OnCollisionExit(Collision collision)
    {
        midAir = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (checkBeneath())
        {
            midAir = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        cameraHolder.transform.position = gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0, getCam.yAngle, 0);
        float verInput = Input.GetAxisRaw("Verticle");
        debugValueSys.display("verticle", "Verticle axis: " + verInput);
        float horInput = Input.GetAxisRaw("Horizontal");
        debugValueSys.display("horizontal", "Horizontal axis: " + horInput);
        //walking and strafing midair
        if (verInput != 0 || horInput != 0)
        {
            float yAngleAdjust = 0;
            //Foward
            if (verInput > 0 && horInput == 0)
            {
                yAngleAdjust = 90;
            }
            //Right Foward
            else if (verInput > 0 && horInput > 0)
            {
                yAngleAdjust = 45;
            }
            //Right
            else if (verInput == 0 && horInput > 0)
            {
                yAngleAdjust = 0;
            }
            //Back Right
            else if (verInput < 0 && horInput > 0)
            {
                yAngleAdjust = -45;
            }
            //Back
            else if (verInput < 0 && horInput == 0)
            {
                yAngleAdjust = -90;
            }
            //Back Left
            else if (verInput < 0 && horInput < 0)
            {
                yAngleAdjust = -135;
            }
            //Left
            else if (verInput == 0 && horInput < 0)
            {
                yAngleAdjust = -180;
            }
            else if (verInput > 0 && horInput < 0)
            {
                yAngleAdjust = 135;
            }
            debugValueSys.display("adjust", "Adjust Angle: " + yAngleAdjust);
            float moveDir = yAngleAdjust - gameObject.transform.eulerAngles.y;
            //Strafing midair
            if (midAir)
            {
                float xMove = airSpeed * Mathf.Cos(moveDir * Mathf.Deg2Rad);
                float zMove = airSpeed * Mathf.Sin(moveDir * Mathf.Deg2Rad);
                if((xMove <= 0) != (objectPhysics.velocity.x <= 0))
                {
                    xMove *= midairReversalRatio;
                }
                if ((zMove <= 0) != (objectPhysics.velocity.z <= 0))
                {
                    zMove *= midairReversalRatio;
                }
                Vector3 moveAccel = new Vector3(xMove, 0, zMove);
                objectPhysics.AddForce(moveAccel * Time.deltaTime, ForceMode.Acceleration);
            }
            //Walking on ground
            else
            {
                Vector3 moveVel = new Vector3(groundSpeed * Mathf.Cos(moveDir * Mathf.Deg2Rad), 0, groundSpeed * Mathf.Sin(moveDir * Mathf.Deg2Rad));
                objectPhysics.velocity = moveVel;
            }

        }

        float spaInput = Input.GetAxisRaw("Space");
        //jumping
        if (spaInput != 0)
        {
            if (!midAir)
            {
                midAir = true;
                objectPhysics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
