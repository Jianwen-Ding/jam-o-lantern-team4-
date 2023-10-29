using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushButton : buttonScript
{
    // what happens when button is pressed in
    public void pressButton()
    {
        if (isActivated)
        {
            if (isToggle)
            {
                deactivateButton();
            }
        }
        else
        {
            activateButton();
        }
    }
}
