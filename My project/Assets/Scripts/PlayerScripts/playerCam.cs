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
    Transform orientation;

    [SerializeField]
    float xAngle;
    [SerializeField]
    float yAngle;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //Locks mouse to center
        Cursor.lockState = CursorLockMode.Locked;
        //Makes mouse invisible
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensetivity;
    }
}
