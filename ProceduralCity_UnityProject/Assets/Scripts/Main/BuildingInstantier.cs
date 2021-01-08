using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingInstantier
{
    public List<GameObject> grounds;
    public List<GameObject> rdcs;
    public List<GameObject> floors;
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
                    // angle = ComputeAngle(x, y, out connectedRoad);
                    if (worldData[x, y].density > 0.5)
                    {
                        int height = rnd.Next(2, (int)(10 * worldData[x, y].density));
                        CreateBuilding(x, y, height);
                    }
                    else
                    {
                        //CreateHouse(x, y);
                    }


                }
            }
        }
    }

    public void CreateBuilding(int x, int y, int height, int angle = 0, int worldSize = 100)
    {
        // Dropping ground
        GameObject groundPrefab = grounds[rnd.Next(0, grounds.Count)];
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, groundPrefab);
        GameObject g = GameObject.Instantiate(groundPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        g.transform.Rotate(new Vector3(-90, 0, 0));
        g.isStatic = true;
        // Computing Ground height
        Renderer groundRenderer = g.GetComponent<Renderer>();
        float groundLevel = groundRenderer.bounds.size.y;
        // Dropping First floor
        GameObject rdcPrefab = rdcs[rnd.Next(0, rdcs.Count)];
        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, rdcPrefab);
        GameObject rdc = GameObject.Instantiate(rdcPrefab, new Vector3(ActualPos.x, groundLevel, ActualPos.y), Quaternion.identity);
        rdc.transform.Rotate(new Vector3(-90, 0, 0));
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
            floor.transform.Rotate(new Vector3(-90, 0, 0));
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
        rdc.transform.Rotate(new Vector3(-90, 0, 0));
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