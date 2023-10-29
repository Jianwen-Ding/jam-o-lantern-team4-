using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateCapture : MonoBehaviour
{
    #region classes

    public class StateFrameCaptured
    {
        public float frameTime;
        public Vector3 objectPosition;
        public Vector2 angleView;
        public bool grabCommand;
        public bool throwCommand;
        public bool punchCommand;
        // constructor
        public StateFrameCaptured(float fT, Vector3 oP, Vector2 aV, bool gC, bool tC, bool pC) {
            frameTime = fT;
            objectPosition = oP;
            angleView = aV;
            grabCommand = gC;
            throwCommand = tC;
            punchCommand = pC;
        }

        public StateFrameCaptured duplicate()
        {
            return new StateFrameCaptured(frameTime, objectPosition, angleView, grabCommand, throwCommand, punchCommand);
        }
        // constructs empty string
        public StateFrameCaptured()
        {
            frameTime = 0;
            objectPosition = Vector3.zero;
            angleView = Vector2.zero;
            grabCommand = false;
            throwCommand = false;
            punchCommand = false;
        }

        //returns the chopped frame and subtract the rest, GIVEN TIME NEEDS TO BE LESS THAN FRAME TIME
        public StateFrameCaptured choppedAtTime(float givenTime)
        {
            frameTime -= givenTime;
            return new StateFrameCaptured(givenTime, objectPosition, angleView, false, false, false);
        }

    }

    public class StatePlaythroughCaptured
    {
        List<StateFrameCaptured> stateList = new List<StateFrameCaptured>();

        // constructors
        public StatePlaythroughCaptured()
        {
            stateList = new List<StateFrameCaptured>();
        }

        public StatePlaythroughCaptured(StatePlaythroughCaptured givenCopy)
        {
            List<StateFrameCaptured> returnList = new List<StateFrameCaptured>();
            for (int i = 0; i < givenCopy.stateList.Count; i++) {
                returnList.Add(givenCopy.stateList[i].duplicate());
            }
            stateList = returnList;
        }

        public StatePlaythroughCaptured dulplicate()
        {
            return new StatePlaythroughCaptured(this);
        }
        // turns a list of frame states into a single one
        public StateFrameCaptured flattenInputList(List<StateFrameCaptured> stateList)
        {
            if(stateList.Count <= 0)
            {
                print("-ERROR- List has no ");
                return new StateFrameCaptured();
            }
            else
            {
                StateFrameCaptured lastFrame = stateList[stateList.Count - 1];
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
        public StateFrameCaptured chopInputList(float timeGet)
        {
            float timeLeft = timeGet;
            List<StateFrameCaptured> ranThroughList = new List<StateFrameCaptured>();
            while (timeLeft > 0){
                //Has not exausted entire captured playthrough
                if(stateList.Count != 0)
                {
                    StateFrameCaptured currentFrame = stateList.ToArray()[0];
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
        public void addState(StateFrameCaptured addedState)
        {
            debugValueSys.display("list size","listSize: " + stateList.Count);
            stateList.Add(addedState);
        }

        // clears the states and returns them
        public StatePlaythroughCaptured clearStates()
        {
            StatePlaythroughCaptured returnOb = dulplicate();
            stateList.Clear();
            return returnOb;
        }

        // gives count of states
        public int countStates()
        {
            return stateList.Count;
        }

        // gives count of state time -- for debug use
        public float statesTime()
        {
            float timeCount = 0;
            for(int i = 0; i < stateList.Count; i++)
            {
                timeCount += stateList[i].frameTime;
            }
            return timeCount;
        }
        // returns true if the list has nothing in it
        public bool isEmpty()
        {
            return stateList.Count == 0;
        }
    }
    #endregion
    #region vars

    public StatePlaythroughCaptured stateStore = new StatePlaythroughCaptured();
    //Cache get
    playerCam getAngle;
    [SerializeField]
    handController getHand;
    bool started;
    #endregion

    // inputs current commands, angle, and position into a returned StateFrameCaptured
    private StateFrameCaptured captureState()
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector2 currentAngle = getAngle.getAngleVec();
        float time = Time.deltaTime;
        return new StateFrameCaptured(time, currentPos, currentAngle, getHand.hasGrabCommand, getHand.hasThrownCommand, getHand.hasPunchCommand);
    }
    // Start is called before the first frame update
    void Start()
    {
        getAngle = Camera.main.GetComponent<playerCam>();
    }

    public void start()
    {
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            stateStore.addState(captureState());
        }
    }
}
