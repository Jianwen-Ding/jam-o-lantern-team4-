using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fadeInFadeOut : MonoBehaviour
{
    // cache
    static fadeInFadeOut soleFade;
    static Image soleImage;
    static float speed;
    static bool activate = false;
    // if not fade in, fade out
    static bool doesFadeIn = false;

    public static void fadeIn(float getSpeed)
    {
        soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, 0);
        speed = getSpeed;
        activate = true;
        doesFadeIn = true;
    }
    public static void fadeOut(float getSpeed)
    {
        soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, 1);
        speed = getSpeed;
        activate = true;
        doesFadeIn = false;
    }

    void Awake()
    {
        if(FindObjectsOfType<fadeInFadeOut>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            soleFade = this;
            soleImage = gameObject.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            float currentSpeed = speed * Time.deltaTime;
            if (doesFadeIn)
            {
                if(soleImage.color.a + currentSpeed >= 1)
                {
                    activate = false;
                    soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, 1);
                }
                else
                {
                    soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, soleImage.color.a + currentSpeed);
                }
            }
            else
            {
                if (soleImage.color.a - currentSpeed <= 0)
                {
                    activate = false;
                    soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, 0);
                }
                else
                {
                    soleImage.color = new Color(soleImage.color.r, soleImage.color.g, soleImage.color.b, soleImage.color.a - currentSpeed);
                }
            }
        }
    }
}
