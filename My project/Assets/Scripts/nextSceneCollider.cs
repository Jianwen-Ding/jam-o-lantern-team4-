using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextSceneCollider : MonoBehaviour
{
    bool activated = false;
    [SerializeField]
    float timeUntilChange;
    float timeLeft = 0;
    [SerializeField]
    string sceneTravelTo;

    // Begins to transition upon player entering
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            activated = true;
            timeLeft = timeUntilChange;
            fadeInFadeOut.fadeIn(1 / timeUntilChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                if(timeLeft <= 0)
                {
                    SceneManager.LoadScene(sceneTravelTo);
                }
            }
        }
    }
}
