using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateCapture : MonoBehaviour
{
    #region classes
    public class actionsGiven
    {
        public float timeIntoState;
        public string currentState;
    }

    public class stateFrameCaptured
    {
        public float frameTime;
        public Vector3 objectPosition;
        public Vector2 angleView;
        public List<actionsGiven> givenActions;
        //Constructor
        public stateFrameCaptured(float fT, Vector2 oP, Vector2 aV, List<actionsGiven> gA) {
            frameTime = fT;
            objectPosition = oP;
            angleView = aV;
            givenActions = gA;
        }
        //returns the chopped frame and subtract the rest, GIVEN TIME NEEDS TO BE LESS THAN FRAME TIME
        public stateFrameCaptured choppedAtTime(float givenTime)
        {
            List<actionsGiven> modifyAction = givenActions;
            for(int i = 0; i < modifyAction.Count; i++)
            {
                modifyAction[i].timeIntoState = modifyAction[i].timeIntoState - frameTime + givenTime;
            }
            frameTime -= givenTime;
            return new stateFrameCaptured(givenTime, objectPosition, angleView, modifyAction);
        }

    }

    public class StatePlaythroughCaptured
    {
        List<stateFrameCaptured> inputList;
        //Gives a list of state frames that happened before the time given and chops them off
        public List<stateFrameCaptured> chopInputList(float timeGet)
        {
            float timeLeft = timeGet;
            List<stateFrameCaptured> ranThroughList = new List<stateFrameCaptured>();
            while (timeLeft > 0){
                //Has not exausted entire captured playthrough
                if(inputList.Count != 0)
                {
                    stateFrameCaptured currentFrame = inputList.ToArray()[0];
                    float timeGiven = inputList[0].frameTime;
                    //Found frame that happens at time
                    if (timeLeft < timeGiven)
                    {
                        //chops frame arrived at in half
                        ranThroughList.Add(currentFrame.choppedAtTime(timeLeft));
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

        public void addState(stateFrameCaptured addedState)
        {
            inputList.Add(addedState);
        }
    }
    #endregion
    #region vars

    StatePlaythroughCaptured stateStore;
    #endregion

    private stateFrameCaptured captureState()
    {
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateStore.addState(captureState());
    }
}
