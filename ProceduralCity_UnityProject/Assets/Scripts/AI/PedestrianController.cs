using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PedestrianController : MonoBehaviour
{
    Rigidbody rigidBody;

    private float power = 1;
    private float torque = .5f;
    private float maxSpeed = 10;
    private Vector2 movement;
    
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 target)
    {
        this.movement = target;
        this.movement.y = 3f;
    }

    private void FixedUpdate()
    {
        if (rigidBody.velocity.magnitude < maxSpeed)
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
