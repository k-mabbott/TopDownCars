using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    // Get score


    //Components
    Rigidbody2D carRigidbody2D;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Frame-rate Independent for physics
    void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

    }
    void ApplyEngineForce()
    {
        //Calc how much forward we are going
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //Limit so we cant go past max speed
        if (velocityVsUp > maxSpeed && accelerationInput < 0)
            return;
        //Limit so we cant go past max/2 in reverse 
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;
        //Limit any other direction
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;


        //Apply drag if no accel input
        if (accelerationInput == 0)
        {
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.5f, Time.fixedDeltaTime * 3);
        }
        else
        {
            carRigidbody2D.drag = 0;
        }

        // Creates force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //Apply force and push car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {

        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Update the rotation angle based on steering
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating car
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity()
    {
        // Returns how fast the car is moving sideways
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }


    public bool IsTireSchreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        // Checks if moving and brakes are applied
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        } // Checks if the car is sliding sideways
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;
        }
        // If none of the above dont render marks
        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }


}
