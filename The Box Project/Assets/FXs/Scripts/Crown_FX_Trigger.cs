using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown_FX_Trigger : MonoBehaviour
{

    Material fxMaterial;
    ParticleSystem crownFx;
    public ParticleSystem burstFx; 

    public Color blueParticle;
    public Color redParticle;
    public Color yellowParticle;

    [ColorUsageAttribute(true, true)]
    public Color blueMat;
    [ColorUsageAttribute(true, true)]
    public Color redMat;
    [ColorUsageAttribute(true, true)]
    public Color yellowMat;


    // Start is called before the first frame update
    void Start()
    {



        crownFx = GetComponent<ParticleSystem>(); 
        fxMaterial = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("X Button"))
        {
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            main.startColor = blueParticle;

            fxMaterial.SetColor("_Color", blueMat);

            ParticleSystem.MainModule main02 = burstFx.GetComponent<ParticleSystem>().main;
            main02.startColor = blueParticle;
            burstFx.Play();




        }

        if (Input.GetButtonDown("B Button"))
        {
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            main.startColor = redParticle;

            fxMaterial.SetColor("_Color", redMat);

            ParticleSystem.MainModule main02 = burstFx.GetComponent<ParticleSystem>().main;
            main02.startColor = redParticle;
            burstFx.Play();

        }

        if (Input.GetButtonDown("Y Button"))
        {
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            main.startColor = yellowParticle;

            fxMaterial.SetColor("_Color", yellowMat);

            ParticleSystem.MainModule main02 = burstFx.GetComponent<ParticleSystem>().main;
            main02.startColor = yellowParticle;
            burstFx.Play();

        }

    }
}
