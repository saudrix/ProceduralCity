using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionList
{
    GoToWork,
    Idle,
    GoToHome,
    Sleep
}

public class Inhabitant : MonoBehaviour
{
    public Schedule planning;

    public GameObject livingPlace;
    public GameObject workPlace;

    public List<Waypoint> roadToWork;

    bool asCar = true;

    void Start()
    {
        
    }

    void Update()
    {
        Act(Mathf.RoundToInt(WorldManager.timeOfDay));
    }

    private void Act(int timeOfDay)
    {
        ActionList action = planning.actions[timeOfDay];
        switch(action)
        {
            case ActionList.GoToWork:
                break;
        }
    }

    // Camera rig follow path reference
    private void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }
}
