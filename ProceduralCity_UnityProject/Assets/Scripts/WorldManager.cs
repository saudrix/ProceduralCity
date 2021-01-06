using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
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
    //public BuildingInstantier buildingInstantier = new BuildingInstantier();
    public RoadConnector roadConnector = new RoadConnector();

    void Start()
    {
        worldData = new SimData[worldSize, worldSize];

        //mapGenerator.GenerateMap(worldData);
        //roadInstantier.CreateRoads(worldData);
        //buildingInstantiater.CreateBuildings(worldData);
        //roadConnector.ConnectRoads(worldData);
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (enableDebug && worldData != null)
            for (int x = 0; x < worldSize; x++)
                for (int y = 0; y < worldSize; y++)
                {
                    if (worldData[x, y].repr != null)
                    {
                        if (worldData[x, y].repr == "I") Gizmos.color = Color.magenta;
                        else if (worldData[x, y].repr == "SV" || worldData[x, y].repr == "SH") Gizmos.color = Color.cyan;
                        else if (worldData[x, y].repr.Contains("T")) Gizmos.color = Color.red;
                        else if (worldData[x, y].repr.Contains("E")) Gizmos.color = Color.green;
                        else if (worldData[x, y].repr.Contains("C")) Gizmos.color = Color.yellow;
                        else if (worldData[x, y].repr == "A") Gizmos.color = Color.blue;
                        else if (worldData[x, y].repr == "H") Gizmos.color = Color.grey;
                        else if (worldData[x, y].repr == "R") Gizmos.color = Color.black;
                        Gizmos.DrawSphere(new Vector3(x - worldSize / 2, 0, y - worldSize / 2), 0.5f);
                    }
                    if(drawDensity)
                    {
                        Gizmos.color = Color.gray * worldData[x, y].density;
                        Gizmos.DrawSphere(new Vector3(x - worldSize / 2, 0, y - worldSize / 2), 0.5f);
                    }
                }
    }

}
