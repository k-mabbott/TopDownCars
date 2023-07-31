using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{
    // [Header("Mixers")]
    // public AudioMixer audioMixer;


    [Header("Audio sources")]
    public AudioSource tireScreechingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource carHitAudioSource;

    // Local variable
    float tireScreechPitch = 0.5f;
    float desiredEnginePitch = 0.5f;


    //Components
    CarController carController;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // audioMixer.SetFloat("SFX", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        // Handle SFC
        float velocityMagnitude = carController.GetVelocityMagnitude();
        // Increase volume with speed
        float desiredEngineVolume = velocityMagnitude * 0.05f;
        // Dont let it get go over or silent
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 0.5f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);

    }

    void UpdateTiresScreechingSFX()
    {
        //Handle tire screeching SFX
        if (carController.IsTireSchreeching(out float lateralVelocity, out bool isBraking))
        {
            //If car is braking
            if(isBraking)
            {
                tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                //If we are not braking but we are drifting
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        //Fade noise out slowly
        else tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime * 15);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Get velocity of collision
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;

        if(!carHitAudioSource.isPlaying)
        {
            carHitAudioSource.Play();
        }
    }
}
