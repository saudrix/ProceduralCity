using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    Rigidbody rigidBody;

    private float power = 20;
    private float torque = 8f;
    private float maxSpeed = 50;
    private Vector2 movement;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Drive(Vector2 target)
    {
        this.movement = target;
    }

    private void FixedUpdate()
    {
        if(rigidBody.velocity.magnitude < maxSpeed)
        {
            rigidBody.AddForce(movement.y * transform.forward * power);
        }
        rigidBody.AddTorque(movement.x * Vector3.up * torque * movement.y);
    }

    private void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }
}
