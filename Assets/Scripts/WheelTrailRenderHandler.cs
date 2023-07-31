using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderHandler : MonoBehaviour
{

    CarController carController;
    TrailRenderer trailRenderer;


    void Awake()
    {
        // Get controler and trail renderer
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();

        // Set emitter to not emit at the start
        trailRenderer.emitting = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check for TiresScreeching from func on controller
        if(carController.IsTireSchreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        } else {
            trailRenderer.emitting = false;
        }
    }
}
