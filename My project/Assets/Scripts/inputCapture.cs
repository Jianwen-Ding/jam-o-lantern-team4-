using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputCapture : MonoBehaviour
{
    #region classes
    public class InputFrameCaptured
    {
        public float frameTime;
        public float verticleInput;
        public float horizontalInput;
        public float spaceInput;
        public float grabInput;
        public float throwInput;
        public float punchInput;
        public Vector2 angleView;
        public InputFrameCaptured(float fT, float vT, float hT, float sI, float gI, float tI, float pI, Vector2 aV) {
            frameTime = fT;
            verticleInput = vT;
            horizontalInput = hT;
            spaceInput = sI;
            grabInput = gI;
            throwInput = tI;
            punchInput = pI;
            angleView = aV;
        }
    }

    public class InputPlaythroughCaptured
    {
        List<InputFrameCaptured> inputList;
        public List<InputFrameCaptured> chopInputList(float timeGet)
        {
            float timeLeft = timeGet;
            List<InputFrameCaptured> ranThroughList = new List<InputFrameCaptured>();
            while (timeLeft > 0){
                //Has not exausted entire captured playthrough
                if(inputList.Count != 0)
                {
                    InputFrameCaptured currentFrame = inputList.ToArray()[0];
                    float timeGiven = inputList[0].frameTime;
                    //Found frame that happens at time
                    if (timeLeft < timeGiven)
                    {
                        InputFrameCaptured modifyFrame = currentFrame;
                        modifyFrame.frameTime = timeLeft;
                        ranThroughList.Add(modifyFrame);
                        inputList[0].frameTime =- timeLeft;
                        break;
                    }
                    //Frame took too long and moving onto next frame
                    else
                    {
                        ranThroughList.Add(inputList.ToArray()[0]);
                        timeLeft = timeLeft - timeGiven;
                        inputList.RemoveAt(0);
                    }
                }
                //Has exausted entire captured playthrough and returns nothing
                else
                {
                    break;
                }
                
            }
            return ranThroughList;
        }
    }
    #endregion
    /*private captureInput()
    {

    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
