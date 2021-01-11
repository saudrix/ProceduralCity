using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarAI : MonoBehaviour
{
    public event Action AsArrived;

    [SerializeField]
    private List<Waypoint> path;
    [SerializeField]
    private GameObject rayStartingPoint = null;
    private float safetyDistance = .2f;

    private float distanceThreshold = .4f;
    private float arrivalThreshold = .3f;
    private float angleOffset = 2;

    private Vector3 currentTarget;

    private int index = 0;

    private bool stop;
    private bool collisionStop = false;

    public bool Stop
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    public bool IsArrived()
    {
        return index >= path.Count - 1;
    }

    private void Start()
    {
        if (path == null || path.Count == 0) Stop = true;
        else
        {
            currentTarget = path[index].transform.position;//.GetPosition();
        }
    }

    public void setPath(List<Waypoint> path)
    {
        if (path.Count == 0)
        {
            // To be modified To instantiate a person
            Destroy(gameObject);
            return;
        }
        this.path = path;
        index = 0;
        currentTarget = path[index].transform.position;//.GetPosition();

        Vector3 relative = transform.InverseTransformPoint(this.path[index + 1].transform.position/*.GetPosition()*/);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;
    }

    private void Update()
    {
        checkArrival();
        Drive();
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        Ray ray = new Ray(rayStartingPoint.transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayStartingPoint.transform.position, transform.forward, safetyDistance, 1 << gameObject.layer))
        {
            collisionStop = true;
        }
        else if (Physics.Raycast(ray, out hit, safetyDistance))
        {
            if (hit.collider.isTrigger && hit.collider.enabled == true)
            {
                if (hit.transform.gameObject.CompareTag("trafficLight"))
                    collisionStop = true;
            }
        }
        else{
            collisionStop = false;
        }
    }

    private void Drive()
    {
        if (Stop)
            OnDrive?.Invoke(Vector2.zero);
        else
        {
            Vector3 relative = transform.InverseTransformPoint(currentTarget);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

            var rotateCar = 0;
            if (angle > angleOffset) rotateCar = 1;
            else rotateCar = -1;

            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void checkArrival()
    {
        if(!Stop)
        {
            float Threshold = distanceThreshold;
            if (index == path.Count - 1) Threshold = arrivalThreshold;

            if (Vector3.Distance(currentTarget, transform.position) < Threshold)
                GetNextTarget();
        }
    }

    private void GetNextTarget()
    {
        index++;
        if (index >= path.Count)
        {
            Stop = true;
            AsArrived?.Invoke();
        }
        else
        {
            currentTarget = path[index].transform.position;//.GetPosition();
        }
    }
}
