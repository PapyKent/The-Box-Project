using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms_Color_State: MonoBehaviour
{
    // What is the color of the active platforms ? (Numpad 1 = blue, 2 = red, 3 = yellow)
    public bool isBlue = false;
    public bool isRed = false;
    public bool isYellow = false;

    // A timer for each color. This is used when a platform disappear (it's animating the dissolve parameter)
    [HideInInspector]
    public float blueTimer;
    [HideInInspector]
    public float redTimer;
    [HideInInspector]
    public float yellowTimer;

    // The speed of the dissolve animation when a platform dissapears.
    public float platformDissolveSpeed = 3; 


    // Update is called once per frame
    void Update()
    {

        // Increase each timer

        blueTimer += platformDissolveSpeed * Time.deltaTime;
        redTimer += platformDissolveSpeed * Time.deltaTime;
        yellowTimer += platformDissolveSpeed * Time.deltaTime;

        // Restart each timer when the Key activating another color state is pressed

        if (Input.GetKeyDown(KeyCode.Keypad1) && isYellow ==true || Input.GetKeyDown(KeyCode.Keypad2) && isYellow == true)
        {
            yellowTimer = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) && isRed == true || Input.GetKeyDown(KeyCode.Keypad3) && isRed == true)
        {
            redTimer = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) && isBlue == true || Input.GetKeyDown(KeyCode.Keypad3) && isBlue == true)
        {
            blueTimer = 0.0f;
        }

        // Change the active color by pressing a Key (Numpad 1 = blue, 2 = red, 3 = yellow)

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            isBlue = true;
            isRed = false;
            isYellow = false; 
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            isBlue = false;
            isRed = true;
            isYellow = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            isBlue = false;
            isRed = false;
            isYellow = true;
        }

    }
}
