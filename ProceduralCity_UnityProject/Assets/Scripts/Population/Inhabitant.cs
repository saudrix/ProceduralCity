using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public event Action leftWork;
    public event Action comeWork;


    public Schedule planning;

    public Housing livingPlace;
    public Housing workPlace;

    public List<Waypoint> roadToWork;
    public List<Waypoint> roadToHome;

    GameObject car;
    CarAI carController;

    ActionList lastAction;

    public bool asCar = true;

    WaypointsAStar AstarFinder = new WaypointsAStar("starFinder");

    public void SetCar(GameObject car)
    {
        this.car = car;
        carController = car.GetComponent<CarAI>();
    }


    void Start()
    {
        car.SetActive(false);
        lastAction = planning.actions[(int)WorldManager.timeOfDay];

        Debug.Log(livingPlace);
        Debug.Log(livingPlace.CarPosition);
        Debug.Log(workPlace);
        Debug.Log(workPlace.CarPosition);

        /*if (asCar)
        {
            // Way to work
            //List<Waypoint> aStarResult = AstarFinder.AStar(livingPlace.CarPosition, workPlace.CarPosition);
            List<Waypoint> aStarResult = await Task.Run(() => AstarFinder.AStar(livingPlace.CarPosition, workPlace.CarPosition));

            if(aStarResult != null)
            {
                aStarResult.Reverse();
                roadToWork = aStarResult;

                // way home
                aStarResult = await Task.Run(() => AstarFinder.AStar(workPlace.CarPosition, livingPlace.CarPosition));
                
                if (aStarResult != null)
                {
                    aStarResult.Reverse();
                    roadToHome = aStarResult;
                }
                else
                {
                    Kill();
                }
            }
            else Kill();
        }*/
    }

    void Update()
    {
        Act(Mathf.RoundToInt(WorldManager.timeOfDay));
    }

    private void Act(int timeOfDay)
    {
        ActionList action = planning.actions[timeOfDay];
        if(action != lastAction)
        {

            Debug.Log(action);
            switch (action)
            {
                case ActionList.GoToWork:
                    leftHome?.Invoke();
                    car.SetActive(true);
                    car.transform.position = livingPlace.CarPosition.transform.position;
                    //carController.setPath(roadToWork);
                    carController.GoTo(livingPlace.CarPosition, workPlace.CarPosition);
                    carController.AsArrived += ArrivedToWork;
                    break;

                case ActionList.GoToHome:
                    leftWork?.Invoke();
                    car.SetActive(true);
                    car.transform.position = workPlace.CarPosition.transform.position;
                    //carController.setPath(roadToHome);
                    carController.GoTo(workPlace.CarPosition, livingPlace.CarPosition);
                    carController.AsArrived += ArrivedToHome;
                    break;
            }
            lastAction = action;
        }
    }

    private void ArrivedToHome()
    {
        car.SetActive(false);
        carController.AsArrived -= ArrivedToHome;
        comeHome?.Invoke();
    }

    private void ArrivedToWork()
    {
        car.SetActive(false);
        carController.AsArrived -= ArrivedToWork;
        comeWork?.Invoke();
        Debug.Log("Arrived at work");
    }

    // Camera rig follow path reference
    private void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }

    void Kill()
    {
        Destroy(car);
        Destroy(this.gameObject);
        Debug.Log("KILLED");
    }
}
