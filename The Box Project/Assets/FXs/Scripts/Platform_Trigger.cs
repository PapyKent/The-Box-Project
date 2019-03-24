using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Trigger : MonoBehaviour
{

    // This script is on the Mesh Renderer of each platform (look inside the prefab for the Platfrom_"Color" Game Object).



    Platforms_Color_State colorState;

    // Particles system to play when the platform is On or Off
    public ParticleSystem onParticles;
    public ParticleSystem offParticles;

    // This is used to set the material parameters when the platform is On or Off
    Material instancedMaterial;
    Renderer meshRenderer;

    // You have to chose in the Inspector the color of the platform
    public bool isBlue; 
    public bool isRed;
    public bool isYellow;

    // The timer used for animating the dissolve parameter
    float t;


    // Start is called before the first frame update
    void Start()
    {

        colorState = GameObject.Find("Platforms_ColorState").GetComponent<Platforms_Color_State>();

        meshRenderer = gameObject.GetComponent<Renderer>();
        instancedMaterial = meshRenderer.material;

    }

    // Update is called once per frame
    void Update()
    {

        // Grabbing the color timers from the Color_state script

        if (isBlue == true)
        {
            t = colorState.blueTimer;
        }
        if (isRed == true)
        {
            t = colorState.redTimer;
        }
        if (isYellow == true)
        {
            t = colorState.yellowTimer;
        }



        //BLUE PLATFORM : This happen only if isBlue is true 

        // This happens only if the Key is pressed + if the blue platforms are Off
        if (isBlue == true && Input.GetKeyDown(KeyCode.Keypad1) && colorState.isBlue == false)
        {
            onParticles.Play();
        }

        // This happens only if the Key for another color state is pressed
        if (isBlue == true && colorState.isBlue == true && Input.GetKeyDown(KeyCode.Keypad2) || isBlue == true && colorState.isBlue == true && Input.GetKeyDown(KeyCode.Keypad3))
        {
            offParticles.Play();
        }

        // This happens when the color state is NOT blue
        if (isBlue == true && colorState.isRed == true || isBlue == true && colorState.isYellow == true)
        {
            // animates the Dissolve parameter using the color timer to lerp between values
            instancedMaterial.SetFloat("_Dissolveamount", Mathf.Lerp(1.8f, 2.6f, t));
            instancedMaterial.SetFloat("_UVScale", 8.75f);
        }

        // This happens when the color state IS blue
        if (isBlue == true && colorState.isBlue == true)
        {
            instancedMaterial.SetFloat("_Dissolveamount", 1.7f);
            instancedMaterial.SetFloat("_UVScale", 33f);
        }



        //RED PLATFORM : This happen only if isRed is true 

        // This happens only if the Key is pressed + if the red platforms are Off
        if (isRed == true && Input.GetKeyDown(KeyCode.Keypad2) && colorState.isRed == false)
        {
            onParticles.Play();
        }

        // There's no need for Particle System when red platforms dissapear

        // This happens when the color state is NOT red
        if (isRed == true && colorState.isBlue == true || isRed == true && colorState.isYellow == true)
        {
            // animates the Dissolve parameter using the color timer to lerp between values
            instancedMaterial.SetFloat("_Dissolveamount", Mathf.Lerp(1.8f, 2.2f, t));
            instancedMaterial.SetFloat("_UVScale", 2.2f);
        }

        // This happens when the color state IS red
        if (isRed == true && colorState.isRed == true)
        {
            instancedMaterial.SetFloat("_Dissolveamount", 1.57f);
            instancedMaterial.SetFloat("_UVScale", 10.82f);
        }



        //YELLOW PLATFORM : This happen only if isYellow is true 

        // This happens only if the Key is pressed + if the yellow platforms are Off
        if (isYellow == true && Input.GetKeyDown(KeyCode.Keypad3) && colorState.isYellow == false)
        {
            onParticles.Play();
        }

        // This happens when the color state is NOT blue
        if (isYellow == true && colorState.isYellow == true && Input.GetKeyDown(KeyCode.Keypad1) || isYellow == true && colorState.isYellow == true && Input.GetKeyDown(KeyCode.Keypad2))
        {
            offParticles.Play();
        }

        // This happens when the color state is NOT yellow
        if (isYellow == true && colorState.isRed == true || isYellow == true && colorState.isBlue == true)
        {

            // animates the Dissolve parameter using the color timer to lerp between values
            instancedMaterial.SetFloat("_Dissolveamount", Mathf.Lerp(2f, 2.27f, t));
            instancedMaterial.SetFloat("_UVScale", 2.67f);
        }

        // This happens when the color state IS yellow
        if (isYellow == true && colorState.isYellow == true)
        { 
            instancedMaterial.SetFloat("_Dissolveamount", 1.75f);
            instancedMaterial.SetFloat("_UVScale", 13f);
        }
        

    }
}
