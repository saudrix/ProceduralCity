using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingInstantier
{
    public GameObject buildParent;

    public List<GameObject> grounds;
    public List<GameObject> rdcs;
    public List<GameObject> floors1;
    public List<GameObject> floors2;
    public List<GameObject> houses;

    public int dropoutRate = 3;

    System.Random rnd = new System.Random();

    public List<GameObject> CreateBuildings(SimData[,] worldData)
    {
        List<GameObject> buildings = new List<GameObject>();
        int worldSize = worldData.GetLength(0);
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                // If we find a road
                if (worldData[x, y].repr == "H")
                {
                    if (rnd.Next(10) > dropoutRate)
                    {
                        GameObject connectedRoad;
                        int rotation;

                        if (ComputeAngle(x, y, worldData, out connectedRoad, out rotation))
                        {
                            if (connectedRoad != null)
                            {
                                if (worldData[x, y].density > 0.5)
                                {
                                    int height = rnd.Next(2, (int)(10 * worldData[x, y].density));
                                    buildings.Add(CreateBuilding(x, y, height, connectedRoad, rotation, worldData[x, y].density));
                                }
                                else
                                {
                                    buildings.Add(CreateHouse(x, y, connectedRoad, rotation, worldData[x, y].density));
                                }
                            }
                        }
                    }
                }
            }
        }
        return buildings;
    }

    private void AsignWaypoint(GameObject build, GameObject road, int angle)
    {
        Housing housing = build.GetComponent<Housing>();
        if (angle == 0)
        {
            //Waypoint w = road.transform.Find("RightIn").GetComponent<Waypoint>();
            GameObject w = FindEntryPoint(road, new List<string>() { "LeftIn", "TopOut" });
            if (w != null) housing.CarPosition = w.GetComponent<Waypoint>();
        }
        else if (angle == 90)
        {
            //Waypoint w = road.transform.Find("TopIn").GetComponent<Waypoint>();
            GameObject w = FindEntryPoint(road, new List<string>() { "BottomIn", "RightOut" });
            if (w != null) housing.CarPosition = w.GetComponent<Waypoint>();
        }
        else if (angle == 180)
        {
            //Waypoint w = road.transform.Find("LeftIn").GetComponent<Waypoint>();
            GameObject w = FindEntryPoint(road, new List<string>() { "RightIn", "BottomOut" });
            if (w != null) housing.CarPosition = w.GetComponent<Waypoint>();
        }
        else if (angle == 270)
        {
            //Waypoint w = road.transform.Find("BottomIn").GetComponent<Waypoint>();
            GameObject w = FindEntryPoint(road, new List<string>() { "TopIn", "LeftOut" });
            if (w != null) housing.CarPosition = w.GetComponent<Waypoint>();
        }
    }

    private GameObject FindEntryPoint(GameObject parent, List<string> names)
    {
        foreach (string name in names)
        {
            Transform waypoint = parent.transform.Find(name);
            if (waypoint != null) return waypoint.gameObject;
        }
        return null;
    }

    private bool ComputeAngle(int x, int y, SimData[,] worldData, out GameObject connectedRoad, out int rot)
    {
        int worldSize = worldData.GetLength(0);

        List<GameObject> surroundingRound = new List<GameObject>();
        List<int> rotations = new List<int>();

        List<string> targetChar = new List<string>() { "R", "SV", "SH", "T1", "T2", "T3", "T4",
                                                       "E1", "E2","E3", "E4", "C1", "C2", "C3",
                                                       "C4" };

        if (y - 1 > 0 && targetChar.Contains(worldData[x, y - 1].repr))
        {
            rotations.Add(0);
            surroundingRound.Add(worldData[x, y - 1].model);
        }
        if (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr))
        {
            rotations.Add(180);
            surroundingRound.Add(worldData[x, y + 1].model);
        }
        if (x - 1 > 0 && targetChar.Contains(worldData[x - 1, y].repr))
        {
            rotations.Add(90);
            surroundingRound.Add(worldData[x - 1, y].model);
        }
        if (x + 1 < worldSize && targetChar.Contains(worldData[x + 1, y].repr))
        {
            rotations.Add(270);
            surroundingRound.Add(worldData[x + 1, y].model);
        }

        int choice = rnd.Next(0, rotations.Count);
        if (rotations.Count > 0)
        {
            connectedRoad = surroundingRound[choice];
            rot = rotations[choice];
            return true;
        }
        else {
            connectedRoad = null;
            rot = 0;
            return false;
        }
    }

    public GameObject CreateBuilding(int x, int y, int height, GameObject connectedRoad, int angle, float density, int worldSize = 100)
    {
        // Choose style
        List<GameObject> floors = rnd.Next(0, 2) == 0 ? floors1 : floors2;
        // Dropping ground
        GameObject groundPrefab = grounds[rnd.Next(0, grounds.Count)];
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, groundPrefab);
        GameObject g = GameObject.Instantiate(groundPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        g.transform.Rotate(new Vector3(-90, angle, 0));
        g.transform.parent = buildParent.transform;
        g.isStatic = true;
        // Computing Ground height
        Renderer groundRenderer = g.GetComponent<Renderer>();
        float groundLevel = groundRenderer.bounds.size.y;
        // Dropping First floor
        GameObject rdcPrefab = rdcs[rnd.Next(0, rdcs.Count)];
        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, rdcPrefab);
        GameObject rdc = GameObject.Instantiate(rdcPrefab, new Vector3(ActualPos.x, groundLevel, ActualPos.y), Quaternion.identity);
        rdc.transform.Rotate(new Vector3(-90, angle, 0));
        rdc.transform.parent = g.transform;
        rdc.isStatic = true;
        // Computing First floor height
        Renderer rdcRenderer = rdc.GetComponent<Renderer>();
        float rdcLevel = rdcRenderer.bounds.size.y;
        // Dropping Levels
        for (int i = 1; i < height; i++)
        {
            float yPos = i * rdcLevel + groundLevel; // Computing floor position
            GameObject floorPrefab = floors[rnd.Next(0, floors.Count)];
            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, floorPrefab);
            GameObject floor = GameObject.Instantiate(floorPrefab, new Vector3(ActualPos.x, yPos, ActualPos.y), Quaternion.identity);
            floor.transform.Rotate(new Vector3(-90, angle, 0));
            floor.transform.parent = rdc.transform;
            floor.isStatic = true;
        }

        AsignWaypoint(rdc, connectedRoad, angle);
        Housing housing = rdc.GetComponent<Housing>();
        housing.nbFloors = height;
        housing.density = density;

        return rdc;
    } 

    public GameObject CreateHouse(int x, int y, GameObject connectedRoad, int angle, float density, int worldSize = 100)
    {
        // Dropping ground
        GameObject groundPrefab = grounds[rnd.Next(0, grounds.Count)];
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, groundPrefab);
        GameObject g = GameObject.Instantiate(groundPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        g.transform.Rotate(new Vector3(-90, 0, 0));
        g.transform.parent = buildParent.transform;
        // Computing Ground height
        Renderer groundRenderer = g.GetComponent<Renderer>();
        float groundLevel = groundRenderer.bounds.size.y;
        // Dropping First floor
        GameObject housePrefab = houses[rnd.Next(0, houses.Count)];
        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, housePrefab);
        GameObject rdc = GameObject.Instantiate(housePrefab, new Vector3(ActualPos.x, groundLevel, ActualPos.y), Quaternion.identity);
        rdc.transform.Rotate(new Vector3(-90, angle, 0));
        rdc.transform.parent = g.transform;

        AsignWaypoint(rdc, connectedRoad, angle);
        Housing housing = rdc.GetComponent<Housing>();
        housing.nbFloors = 1;
        housing.density = density;

        return rdc;
    }

    public Vector2Int ComputePosition(Vector2Int coordinates, int worldSize, GameObject road)
    {
        Renderer roadRenderer = road.GetComponent<Renderer>();
        float sizeX = roadRenderer.bounds.size.x;
        float sizeY = roadRenderer.bounds.size.z;
        int newX = (coordinates.x - worldSize / 2) - (int)(sizeX / 2);
        int newY = (coordinates.y - worldSize / 2) - (int)(sizeY / 2);

        return new Vector2Int(newX, newY);
    }
}