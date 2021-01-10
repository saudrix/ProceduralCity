using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingInstantier
{
    public List<GameObject> grounds;
    public List<GameObject> rdcs;
    public List<GameObject> floors1;
    public List<GameObject> floors2;
    public List<GameObject> houses;

    System.Random rnd = new System.Random();

    public void CreateBuildings(SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                // If we find a road
                if (worldData[x, y].repr == "H")
                {
                    GameObject connectedRoad;
                    int rotation;

                    if(ComputeAngle(x, y, worldData, out connectedRoad, out rotation))
                    {
                        Debug.Log(rotation);
                        if (worldData[x, y].density > 0.5)
                        {
                            int height = rnd.Next(2, (int)(10 * worldData[x, y].density));
                            CreateBuilding(x, y, height, rotation);
                        }
                        else
                        {
                            CreateHouse(x, y, rotation);
                        }
                    }
                }
            }
        }
    }

    private bool ComputeAngle(int x, int y, SimData[,] worldData, out GameObject connectedRoad, out int rot)
    {
        int worldSize = worldData.GetLength(0);

        List<GameObject> surroundingRound = new List<GameObject>();
        List<int> rotations = new List<int>();

        List<string> targetChar = new List<string>() { "R", "SV", "SH", "T1", "T2", "T3", "T4",
                                                       "E1", "E2","E3", "E4", "C1", "C2", "C3",
                                                       "C4" };

        if(y-1 > 0 && targetChar.Contains(worldData[x,y-1].repr))
        {
            rotations.Add(0);
            surroundingRound.Add(worldData[x, y - 1].model);
        }
        if (y+1 < worldSize && targetChar.Contains(worldData[x, y+1].repr))
        {
            rotations.Add(180);
            surroundingRound.Add(worldData[x, y+1].model);
        }
        if (x-1 > 0 && targetChar.Contains(worldData[x-1, y].repr))
        {
            rotations.Add(90);
            surroundingRound.Add(worldData[x-1, y].model);
        }
        if (x+1 < worldSize && targetChar.Contains(worldData[x+1, y].repr))
        {
            rotations.Add(270);
            surroundingRound.Add(worldData[x+1, y].model);
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

    public void CreateBuilding(int x, int y, int height, int angle = 0, int worldSize = 100)
    {
        // Choose style
        List<GameObject> floors = rnd.Next(0, 2) == 0 ? floors1 : floors2;
        // Dropping ground
        GameObject groundPrefab = grounds[rnd.Next(0, grounds.Count)];
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, groundPrefab);
        GameObject g = GameObject.Instantiate(groundPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        g.transform.Rotate(new Vector3(-90, angle, 0));
        g.isStatic = true;
        // Computing Ground height
        Renderer groundRenderer = g.GetComponent<Renderer>();
        float groundLevel = groundRenderer.bounds.size.y;
        // Dropping First floor
        GameObject rdcPrefab = rdcs[rnd.Next(0, rdcs.Count)];
        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, rdcPrefab);
        GameObject rdc = GameObject.Instantiate(rdcPrefab, new Vector3(ActualPos.x, groundLevel, ActualPos.y), Quaternion.identity);
        rdc.transform.Rotate(new Vector3(-90, angle, 0));
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
            floor.isStatic = true;
        }
    }

    public void CreateHouse(int x, int y, int angle = 0, int worldSize = 100)
    {
        // Dropping ground
        GameObject groundPrefab = grounds[rnd.Next(0, grounds.Count)];
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, groundPrefab);
        GameObject g = GameObject.Instantiate(groundPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        g.transform.Rotate(new Vector3(-90, 0, 0));
        // Computing Ground height
        Renderer groundRenderer = g.GetComponent<Renderer>();
        float groundLevel = groundRenderer.bounds.size.y;
        // Dropping First floor
        GameObject housePrefab = houses[rnd.Next(0, houses.Count)];
        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, housePrefab);
        GameObject rdc = GameObject.Instantiate(housePrefab, new Vector3(ActualPos.x, groundLevel, ActualPos.y), Quaternion.identity);
        rdc.transform.Rotate(new Vector3(-90, angle, 0));
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