using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEmitHandler : MonoBehaviour
{

    public float particleEmissionRate = 0;

    CarController carController;
    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;


    void Awake()
    {
        carController = GetComponentInParent<CarController>();

        particleSystemSmoke = GetComponent<ParticleSystem>();
        //Get emission component
        particleSystemEmissionModule = particleSystemSmoke.emission;
        //Set base to zero
        particleSystemEmissionModule.rateOverTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime*5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if(carController.IsTireSchreeching(out float lateralVelocity, out bool isBraking))
        {
            // If car tires are screeching smoke, emit lots if braking
            if(isBraking)
            {
                particleEmissionRate = 30;
            } else {
                // Playter is drifting emit smoke based on lateral velocity
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }

    }
}
