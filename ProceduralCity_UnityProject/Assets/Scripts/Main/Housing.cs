using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script holds the data for a given house
public class Housing : MonoBehaviour
{
    public bool house;

    public float density;

    public int nbFloors;
    public int nbInhabitants;
    public int nbWorkers;

    public int habitantCount;
    public int workersCount;

    public Waypoint CarPosition;

    void Start()
    {

        habitantCount = 0;
        workersCount = 0;

        int range = Mathf.RoundToInt(density * 10);
        Debug.Log(range);
        if (house) nbInhabitants = Random.Range(1, 5);
        else nbInhabitants = Random.Range(0, range);

        if (house) nbWorkers = Random.Range(0, 2);
        else nbWorkers = Random.Range(10, range * nbFloors);
    }

    public void AddWorker() { workersCount++;  }

    public void AddHabitant() { habitantCount++; }

    public bool ElectableAsLifePlace()
    {
        return habitantCount < nbInhabitants;
    }

    public bool ElectableAsWorkPlace()
    {
        return workersCount < nbWorkers;
    }
}
