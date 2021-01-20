using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PedestrianController))]
public class PedestrianAI : MonoBehaviour
{
    public event Action AsArrived;
    [SerializeField]
    private GameObject rayStartingPoint = null;

    private float safetyDistance = .1f;

    private float distanceThreshold = .3f;
    private float arrivalThreshold = .1f;
    private float angleOffset = 3;

    Waypoint currentTarget;
    Waypoint destination;

    private bool stop;
    private bool collisionStop = false;

    public bool Stop
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnMove { get; set; }

    public void GoTo(Waypoint start, Waypoint destination)
    {
        currentTarget = start;
        this.destination = destination;

        Vector3 relative = transform.InverseTransformPoint(GetNextTarget().transform.position/*.GetPosition()*/);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);

        Stop = false;
    }

    private void Update()
    {
        CheckArrival();
        Move();
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        Ray ray = new Ray(rayStartingPoint.transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayStartingPoint.transform.position, transform.forward, safetyDistance, 1 << gameObject.layer))
        {
            collisionStop = true;
            //Debug.Log("VOITURE");
        }
        else if (Physics.Raycast(ray, out hit, safetyDistance))
        {
            if (hit.collider.isTrigger && hit.collider.enabled == true)
            {
                if (hit.transform.gameObject.CompareTag("trafficLight"))
                    collisionStop = true;
            }
            //Debug.Log("FEU ROUGE");
        }
        else
        {
            collisionStop = false;
            //Debug.Log("NOT COLLIDING");
        }
    }

    private void Move()
    {
        if (Stop)
            OnMove?.Invoke(Vector2.zero);
        else if(currentTarget != null)
        {
            Vector3 relative = transform.InverseTransformPoint(currentTarget.transform.position);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

            var rotateCar = 0;
            if (angle > angleOffset) rotateCar = 1;
            else rotateCar = -1;

            OnMove?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void CheckArrival()
    {
        if (!Stop && currentTarget != null)
        {
            float Threshold = distanceThreshold;
            //if (index == path.Count - 1) Threshold = arrivalThreshold;
            if (currentTarget == destination) Threshold = arrivalThreshold;
            
            if (Vector3.Distance(currentTarget.transform.position, transform.position) < Threshold)
                GetNextTarget();
        }
    }

    private Waypoint GetNextTarget()
    {
        // Needs to be removed when elbow road are added to the sim
        if (currentTarget.next.Count == 0)
        {
            Stop = true;
            currentTarget = null;
            destination = null;
            AsArrived?.Invoke();
        }
        else if (currentTarget == destination)
        {
            Stop = true;
            currentTarget = null;
            destination = null;
            AsArrived?.Invoke();
        }
        else if (currentTarget.next.Count == 1)
        {
            currentTarget = currentTarget.next[0];
        }
        else
        {
            Debug.Log("Actually choosing");
            currentTarget = ClosestPoint();
        }
        return currentTarget;
    }

    Waypoint ClosestPoint()
    {
        float dist = float.MaxValue;
        int max = 0;
        for (int i = 0; i < currentTarget.next.Count; i++)
        {
            float heuristic = EuclideanDist(currentTarget.next[i].transform.position, destination.transform.position);
            if (heuristic < dist)
            {
                dist = heuristic;
                max = i;
            }
        }
        return currentTarget.next[max];
    }

    float EuclideanDist(Vector3 w1, Vector3 w2)
    {
        Debug.Log((float)Math.Sqrt(Math.Pow((w1.x - w2.x), 2) + Math.Pow((w1.z - w2.z), 2)));
        return (float)Math.Sqrt(Math.Pow((w1.x - w2.x), 2) + Math.Pow((w1.z - w2.z), 2));
    }
}
