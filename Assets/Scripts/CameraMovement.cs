using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    // Who do i follow
    public GameObject Car;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - Car.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Car.transform.position + offset;
    }
}
