using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Wheel[] wheels;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turningCircle;
    //all in meters

    [Header("Steering input")]
    public float steerInput;

    public float ackermannAngleLeft, ackermannAngleRight;

    private void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        if(steerInput > 0)
        {
            //turning right
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningCircle + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningCircle - (rearTrack / 2))) * steerInput;
        }
        else if(steerInput < 0)
        {
            //turning left
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningCircle - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningCircle + (rearTrack / 2))) * steerInput;
        }
        else
        {
            //not turning
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach (var w in wheels)
        {
            if(w.frontLeft)
            {
                w.steerAngle = ackermannAngleLeft;
            }
            else if(w.frontRight)
            {
                w.steerAngle = ackermannAngleRight;
            }
        }
    }
}