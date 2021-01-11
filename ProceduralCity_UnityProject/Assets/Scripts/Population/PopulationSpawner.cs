using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PopulationSpawner
{

    public int popSize;
    public List<GameObject> charactersPrefabs;

    WaypointsAStar AstarFinder = new WaypointsAStar("starFinder");

    public void CreatePopulation(List<GameObject> structures)
    {
        for(int i = 0; i < popSize; i++)
        {
            CreatePerson(structures);
        }
    }

    private void CreatePerson(List<GameObject> structures)
    {
        // Choosing a house
        GameObject home = null;
        while(home == null)
        {
            Housing structure = structures[UnityEngine.Random.Range(0, structures.Count - 1)].GetComponent<Housing>();
            if (structure.ElectableAsLifePlace())
            {
                structure.AddHabitant();
                home = structure.gameObject;
            }            
        }
        // Chosing a connectable workplace
        GameObject work = null;
        List<Waypoint> aStarResult = null;
        while(work == null)
        {
            Housing structure = structures[UnityEngine.Random.Range(0, structures.Count - 1)].GetComponent<Housing>();
            if (structure.ElectableAsWorkPlace())
            {
                aStarResult = AstarFinder.AStar(home.GetComponent<Housing>().CarPosition, structure.CarPosition);
                if (aStarResult != null)
                {
                    work = structure.gameObject;
                }
            }

        }
        // Instantiate a person
        GameObject selectedPrefab = charactersPrefabs[UnityEngine.Random.Range(0, charactersPrefabs.Count - 1)];
        GameObject person = GameObject.Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);

        Inhabitant personManager = person.GetComponent<Inhabitant>();
        personManager.livingPlace = home;
        personManager.workPlace = work;
        personManager.roadToWork = aStarResult;
    }
}
