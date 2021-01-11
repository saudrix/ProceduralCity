using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    public static float timeOfDay = 0;
    public int timeManagment = 100;
    // Modular parameters
    [SerializeField]
    bool enableDebug = false;
    bool drawDensity = false;

    // World data information
    [SerializeField]
    int worldSize = 100;
    private SimData[,] worldData;

    // Debug Plane
    // Useful classes
    public MapGenerator mapGenerator = new MapGenerator();
    public RoadInstantier roadInstantier = new RoadInstantier();
    public BuildingInstantier buildingInstantier = new BuildingInstantier();
    public RoadConnector roadConnector = new RoadConnector();
    public PopulationSpawner popSpawner = new PopulationSpawner();

    List<GameObject> structures = new List<GameObject>();

    int debugTime = 0;
    int lastTime = 0;

    void Start()
    {
        worldData = new SimData[worldSize, worldSize];

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.localScale = new Vector3(worldSize / 10, 1, worldSize / 10);
       
        mapGenerator.GenerateMap(worldData);
        roadInstantier.CreateRoads(worldData);
        roadConnector.ConnectRoads(worldData);

        structures = buildingInstantier.CreateBuildings(worldData);

        popSpawner.CreatePopulation(structures); 
    }

    void Update()
    {
        debugTime = Mathf.RoundToInt(timeOfDay);
        timeOfDay += Time.deltaTime / timeManagment;
        timeOfDay %= 24;

        if(lastTime != debugTime)
        {
            Debug.Log("Hour = " + debugTime);
            lastTime = debugTime;
        }
    }
}
