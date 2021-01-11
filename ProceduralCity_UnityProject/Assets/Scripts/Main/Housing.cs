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

    public Waypoint CarPosition;

    void Start()
    {
        int range = Mathf.RoundToInt(density * 10);
        if (house) nbInhabitants = Random.Range(1, 5);
        else nbInhabitants = Random.Range(0, range);

        if (house) nbWorkers = Random.Range(0, 2);
        else nbWorkers = Random.Range(10, range * nbFloors);
    }
}
