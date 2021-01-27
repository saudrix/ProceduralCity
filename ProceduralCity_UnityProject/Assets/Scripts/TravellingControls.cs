using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingControls : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            newPosition  += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            newPosition -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            newPosition += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            newPosition -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(transform.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(transform.up * moveSpeed * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
    }
}
