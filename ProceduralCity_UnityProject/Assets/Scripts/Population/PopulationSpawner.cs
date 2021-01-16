using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PopulationSpawner : MonoBehaviour
{
    public int popSize;

    public List<Schedule> allPlannings;

    public List<GameObject> charactersPrefabs;
    public List<GameObject> carPrefabsBody;
    public GameObject carPrefab;

    WaypointsAStar AstarFinder = new WaypointsAStar("starFinder");

    public void CreatePopulation(List<GameObject> structures)
    {
        StartCoroutine("GraduallyInstantiate", structures);
    }

    IEnumerator GraduallyInstantiate(List<GameObject> structures)
    {
        Debug.Log(structures);
        for (int i = 0; i < popSize; i++) {
            Debug.Log(i);
            CreatePerson(structures);
            yield return new WaitForSeconds(5);
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
            Debug.Log("Searching for a house");
        }
        // Chosing a connectable workplace
        GameObject work = null;
        List<Waypoint> aStarResult = null;
        List<Waypoint> aStarResultReverse = null;

        while(work == null)
        {
            Housing structure = structures[UnityEngine.Random.Range(0, structures.Count - 1)].GetComponent<Housing>();
            if (structure.ElectableAsWorkPlace())
            {
                if(home.GetComponent<Housing>().CarPosition != null && structure.CarPosition != null)
                {
                    aStarResult = AstarFinder.AStar(home.GetComponent<Housing>().CarPosition, structure.CarPosition);
                    aStarResultReverse = AstarFinder.AStar(structure.CarPosition, home.GetComponent<Housing>().CarPosition);
                    if (aStarResult != null && aStarResultReverse != null)
                    {
                        work = structure.gameObject;
                    }
                }
            }
            Debug.Log("Searching for a work place");

        }
        // Instantiate a person
        GameObject selectedPrefab = charactersPrefabs[UnityEngine.Random.Range(0, charactersPrefabs.Count - 1)];
        GameObject person = GameObject.Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);

        Inhabitant personManager = person.GetComponent<Inhabitant>();

        personManager.livingPlace = home.GetComponent<Housing>();
        personManager.workPlace = work.GetComponent<Housing>();

        //home.transform.position = new Vector3(home.transform.position.x, 10, home.transform.position.z);
        //work.transform.position = new Vector3(work.transform.position.x, 10, work.transform.position.z);

        aStarResult.Reverse();
        personManager.roadToWork = aStarResult;
        aStarResultReverse.Reverse();
        personManager.roadToWork = aStarResultReverse;

        personManager.planning = allPlannings[UnityEngine.Random.Range(0, allPlannings.Count-1)];

        // if it has a car creates it
        GameObject carPrefEmpty = GameObject.Instantiate(carPrefab);
        GameObject carBody = GameObject.Instantiate(carPrefabsBody[UnityEngine.Random.Range(0, carPrefabsBody.Count - 1)], new Vector3(0,.1f,0), Quaternion.Euler(0,0,0), carPrefEmpty.transform);
        personManager.SetCar(carPrefEmpty);

        //yield return null;
    }
}
