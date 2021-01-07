using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    Rigidbody rigidBody;

    [SerializeField]
    private float power = 15;
    [SerializeField]
    private float torque = 2f;
    [SerializeField]
    private float maxSpeed = 6;
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
