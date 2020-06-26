using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Rigidbody rb;

    public bool frontLeft, frontRight, rearLeft, rearRight;

    [Header("Suspension")]
    public float restLength;    //(idle) length of the spring
    public float springTravel;  //how much the spring is allowed to shrink/expand, must increase w restlength
    public float springStiffness;   //higher the weight (rb.mass) of the car, higher the stiffness
    public float damperStiffness;   //higher the restlength, higher the stiffness required 
                                    //(since it is a force applied on something, so if the something is bigger,
                                    //it may need more force)

    private float minLength;    //the minimum length the spring can shrink to
    private float maxLength;    //the maximum length the spring can expand
    private float springLength; //spring length in current frame 
    private float springForce;

    private float lastSpringLength; //length of spring in the last frame
    private float springVelocity;  //diff between spring length in last frame and this frame
    private float damperForce;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float wheelRadius;
    public float steerAngle;
    public float steerTime;

    private float wheelAngle;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime); //try lerpAngle here
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + steerAngle, transform.localRotation.z);
    }

    private void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastSpringLength = springLength;

            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            
            springVelocity = (lastSpringLength - springLength) / Time.fixedDeltaTime;

            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            //apply force
            suspensionForce = (springForce + damperForce) * transform.up;

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }
}
