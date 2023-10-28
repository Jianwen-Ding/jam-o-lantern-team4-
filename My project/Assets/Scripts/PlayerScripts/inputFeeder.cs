using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class inputFeeder : MonoBehaviour
{
    //Cache
    [SerializeField]
    GameObject playerHand;
    handMover mover;
    handController handControl;

    //Feeds these states into a clone
    public stateCapture.StatePlaythroughCaptured stateRecording = new stateCapture.StatePlaythroughCaptured();

    // Start
    void Start()
    {
        mover = playerHand.GetComponent<handMover>();
        handControl = playerHand.GetComponent<handController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!stateRecording.isEmpty())
        {
            stateCapture.stateFrameCaptured framesCaptured = stateRecording.chopInputList(Time.deltaTime);
            gameObject.transform.position = framesCaptured.objectPosition;
            gameObject.transform.rotation = Quaternion.Euler(0, framesCaptured.angleView.y, 0);
            mover.givenAngle = framesCaptured.angleView;
            if (framesCaptured.grabCommand)
            {
                handControl.grabCommand(framesCaptured.angleView);
            }
            if (framesCaptured.throwCommand)
            {
                handControl.throwCommand(framesCaptured.angleView);
            }
            if (framesCaptured.punchCommand)
            {
                handControl.punchCommand(framesCaptured.angleView);
            }
        }
    }
}
