using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class debugValueSys : MonoBehaviour
{
    //Cache
    public static GameObject soleGameobject;
    public static debugValueSys soleDebugDisplay;
    public static TextMeshProUGUI soleTextBox;

    //Adress system
    public static List<string> addressesGiven;
    public static List<string> textDisplayed;

    // Start is called before the first frame update
    // Prevents multiple debugValueSys from existing
    void Start()
    {
        if (FindObjectsOfType<debugValueSys>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            addressesGiven = new List<string>();
            textDisplayed = new List<string>();
            soleGameobject = gameObject;
            soleDebugDisplay = this;
            soleTextBox = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }


    //Allows any other script to set up a display on the UGUI
    public static void display(string address, string text)
    {
        bool addressExists = false;
        for(int i = 0; i < addressesGiven.Count; i++)
        {
            if (addressesGiven[i] == address)
            {
                textDisplayed[i] = text;
                addressExists = true;
            }
        }
        if(addressExists == false)
        {
            addressesGiven.Add(address);
            textDisplayed.Add(text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        string textResult = "DEBUG VALUES:";
        for(int i = 0; i < addressesGiven.Count; i++)
        {
             textResult = textResult + "<br><br>" + textDisplayed[i];
        }
        soleTextBox.text = textResult;
    }
}
