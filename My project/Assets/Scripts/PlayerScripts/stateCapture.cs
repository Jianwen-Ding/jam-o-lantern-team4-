using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateCapture : MonoBehaviour
{
    #region classes

    public class stateFrameCaptured
    {
        public float frameTime;
        public Vector3 objectPosition;
        public Vector2 angleView;
        public bool grabCommand;
        public bool throwCommand;
        public bool punchCommand;
        //Constructor
        public stateFrameCaptured(float fT, Vector3 oP, Vector2 aV, bool gC, bool tC, bool pC) {
            frameTime = fT;
            objectPosition = oP;
            angleView = aV;
            grabCommand = gC;
            throwCommand = tC;
            punchCommand = pC;
        }

        //Constructs empty string
        public stateFrameCaptured()
        {
            frameTime = 0;
            objectPosition = Vector3.zero;
            angleView = Vector2.zero;
            grabCommand = false;
            throwCommand = false;
            punchCommand = false;
        }

        //returns the chopped frame and subtract the rest, GIVEN TIME NEEDS TO BE LESS THAN FRAME TIME
        public stateFrameCaptured choppedAtTime(float givenTime)
        {
            frameTime -= givenTime;
            return new stateFrameCaptured(givenTime, objectPosition, angleView, false, false, false);
        }

    }

    public class StatePlaythroughCaptured
    {
        List<stateFrameCaptured> stateList;

        // constructors
        public StatePlaythroughCaptured()
        {
            stateList = new List<stateFrameCaptured>();
        }

        public StatePlaythroughCaptured(List<stateFrameCaptured> givenList)
        {
            print(givenList.Count);
            stateList = new List<stateFrameCaptured>(givenList) ;
        }

        public StatePlaythroughCaptured(StatePlaythroughCaptured givenCopy)
        {
            print(givenCopy.stateList.Count);
            stateList = new List<stateFrameCaptured>(givenCopy.stateList);
        }
        // turns a list of frame states into a single one
        public stateFrameCaptured flattenInputList(List<stateFrameCaptured> stateList)
        {
            if(stateList.Count <= 0)
            {
                print("-ERROR- List has no ");
                return new stateFrameCaptured();
            }
            else
            {
                stateFrameCaptured lastFrame = stateList[stateList.Count - 1];
                for(int i = 0; i < stateList.Count - 1; i++)
                {
                    lastFrame.grabCommand = lastFrame.grabCommand || stateList[i].grabCommand;
                    lastFrame.throwCommand = lastFrame.throwCommand || stateList[i].throwCommand;
                    lastFrame.punchCommand = lastFrame.punchCommand || stateList[i].punchCommand;
                }
                return lastFrame;
            }
        }
        //Gives a list of state frames that happened before the time given and chops them off
        public stateFrameCaptured chopInputList(float timeGet)
        {
            float timeLeft = timeGet;
            List<stateFrameCaptured> ranThroughList = new List<stateFrameCaptured>();
            while (timeLeft > 0){
                //Has not exausted entire captured playthrough
                if(stateList.Count != 0)
                {
                    stateFrameCaptured currentFrame = stateList.ToArray()[0];
                    float timeGiven = stateList[0].frameTime;
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
                        ranThroughList.Add(stateList.ToArray()[0]);
                        timeLeft = timeLeft - timeGiven;
                        stateList.RemoveAt(0);
                    }
                }
                //Has exausted entire captured playthrough and returns nothing
                else
                {
                    break;
                }
                
            }
            return flattenInputList(ranThroughList);
        }

        // adds a state to the inputList
        public void addState(stateFrameCaptured addedState)
        {
            debugValueSys.display("list size","listSize: " + stateList.Count);
            stateList.Add(addedState);
        }

        // clears the states and returns them
        public StatePlaythroughCaptured clearStates()
        {
            StatePlaythroughCaptured returnOb = new StatePlaythroughCaptured(this);
            stateList.Clear();
            return returnOb;
        }

        // gives count of states
        public int countStates()
        {
            return stateList.Count;
        }
        // returns true if the list has nothing in it
        public bool isEmpty()
        {
            return stateList.Equals(new List<stateCapture>());
        }
    }
    #endregion
    #region vars

    public StatePlaythroughCaptured stateStore = new StatePlaythroughCaptured();
    //Cache get
    playerCam getAngle;
    [SerializeField]
    handController getHand;
    #endregion

    private stateFrameCaptured captureState()
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector2 currentAngle = getAngle.getAngleVec();
        float time = Time.deltaTime;
        return new stateFrameCaptured(time, currentPos, currentAngle, getHand.hasGrabCommand, getHand.hasThrownCommand, getHand.hasPunchCommand);
    }
    // Start is called before the first frame update
    void Start()
    {
        getAngle = Camera.main.GetComponent<playerCam>();
    }

    // Update is called once per frame
    void Update()
    {
        stateStore.addState(captureState());
    }
}
