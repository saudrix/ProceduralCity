using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script holds the data for a given house
public class Housing : MonoBehaviour
{
    public Light densityLight;

    public bool house;

    public float density;

    public int nbFloors;
    public int nbInhabitants;
    public int nbWorkers;

    public int habitantCount;
    public int workersCount;

    public Waypoint CarPosition;
    public Waypoint PedestrianPosition;

    void Start()
    {

        habitantCount = 0;
        workersCount = 0;

        int range = Mathf.RoundToInt(density * 10);
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

    public void lightUp()
    {
        if(densityLight.intensity <= workersCount + habitantCount) densityLight.intensity += 1;
    }

    public void lightDown() {
        if(densityLight.intensity > 0) densityLight.intensity -= 1; 
    }
}
