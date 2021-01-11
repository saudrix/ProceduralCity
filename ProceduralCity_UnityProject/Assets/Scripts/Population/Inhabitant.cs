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
    // events raised
    public event Action leftHome;
    public event Action comeHome;

    public Schedule planning;

    public Housing livingPlace;
    public Housing workPlace;

    public List<Waypoint> roadToWork;

    GameObject car;
    CarAI carController;

    public void SetCar(GameObject car)
    {
        this.car = car;
        carController = car.GetComponent<CarAI>();
    }

    bool asCar = true;

    void Start()
    {
        car.SetActive(false);
    }

    void Update()
    {
        Act(Mathf.RoundToInt(WorldManager.timeOfDay));
    }

    private void Act(int timeOfDay)
    {
        Debug.Log(planning.actions[timeOfDay]);

        ActionList action = planning.actions[timeOfDay];
        switch(action)
        {
            case ActionList.GoToWork:
                leftHome?.Invoke();
                car.SetActive(true);
                car.transform.position = livingPlace.CarPosition.transform.position;
                carController.setPath(roadToWork);
                carController.AsArrived += ArrivedToWork;
                break;
        }
    }

    private void ArrivedToWork()
    {
        car.SetActive(false);
        carController.AsArrived -= ArrivedToWork;
        Debug.Log("Arrived To Work");
    }

    // Camera rig follow path reference
    private void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }
}
