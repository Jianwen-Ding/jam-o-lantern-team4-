using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScrollingText : MonoBehaviour
{
    //cache
    GameObject player;
    TextMeshPro text;

    bool engaged = false;
    [SerializeField]
    string textDisplayed;
    int currentLetter = 0;
    [SerializeField]
    float distanceUntilShow;
    [SerializeField]
    float distanceUntilDissapear;
    [SerializeField]
    float timePerLetter;
    float letterTimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, mathHelper.getAngleBetweenVec(gameObject.transform.position, player.transform.position).y + 180, 0);
        float distance = mathHelper.distance(gameObject.transform.position, player.transform.position);
        if (engaged)
        {
            if(distance > distanceUntilDissapear)
            {
                engaged = false;
                currentLetter = 0;
                letterTimeLeft = timePerLetter;
                text.text = "";
            }
            if(currentLetter < textDisplayed.Length)
            {
                letterTimeLeft -= Time.deltaTime;
                if (letterTimeLeft <= 0)
                {
                    char currentChar = textDisplayed[currentLetter];
                    letterTimeLeft = timePerLetter;
                    if(currentChar == ';')
                    {
                        text.text += "<br>";
                    }
                    else
                    {
                        text.text += currentChar;
                    }
                    currentLetter++;
                }
            }
        }
        else
        {
            text.text = "";
            if (distance < distanceUntilShow)
            {
                engaged = true;
            }
        }
    }
}
