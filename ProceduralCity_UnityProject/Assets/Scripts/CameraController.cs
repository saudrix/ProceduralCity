using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform followTransform;
    public Transform cameraTransform;

    public float movementSpeed;
    public float movementTime;
    public float normalSpeed;
    public float fastSpeed;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStart;
    public Vector3 dragPosition;
    public Vector3 rotateStart;
    public Vector3 rotatePosition;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(followTransform != null)
            transform.position = followTransform.position;
        
        else
        {
            HandleMouseInput();
            HandleMovementInput();
        }
        if (Input.GetKey(KeyCode.Escape)) followTransform = null;
    }

    void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hit;

            if(plane.Raycast(ray, out hit))
            {
                dragStart = ray.GetPoint(hit);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hit;

            if (plane.Raycast(ray, out hit))
            {
                dragPosition = ray.GetPoint(hit);
                newPosition = transform.position + dragStart - dragPosition;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotatePosition = Input.mousePosition;
            Vector3 difference = rotateStart - rotatePosition;

            rotateStart = rotatePosition;

            newRotation *= Quaternion.Euler(Vector3.up * -difference.x / 5f);
        }

    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = fastSpeed;
        else movementSpeed = normalSpeed;

        if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            newPosition += (transform.forward * movementSpeed);
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition += (transform.forward * -movementSpeed);
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            newPosition += (transform.right * movementSpeed);
        
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += (transform.right * -movementSpeed);
        
        if (Input.GetKey(KeyCode.A))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        
        if (Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        
        if (Input.GetKey(KeyCode.R))
            newZoom += zoomAmount;

        if (Input.GetKey(KeyCode.F))
            newZoom -= zoomAmount;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
