using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCam : MonoBehaviour
{
    #region vars
    [SerializeField]
    float xSensetivity;
    [SerializeField]
    float ySensetivity;

    [SerializeField]
    public float xAngle;
    [SerializeField]
    public float yAngle;

    public Vector2 getAngleVec()
    {
        return new Vector2(xAngle, yAngle);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //Locks mouse to center
        Cursor.lockState = CursorLockMode.Locked;
        //Makes mouse invisible
        Cursor.visible = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensetivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensetivity;

        yAngle += mouseX;
        xAngle -= mouseY;

        xAngle = Mathf.Clamp(xAngle, -90f, 90f);
        transform.rotation = Quaternion.Euler(xAngle, yAngle, 0);
    }
}
