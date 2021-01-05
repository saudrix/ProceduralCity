using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoadInstantier
{
    public List<GameObject> roadPrefabs;
    public List<GameObject> elbowPrefabs;
    public List<GameObject> tBonePrefabs;
    public GameObject intersectPrefab;

    System.Random random = new System.Random();

    public GameObject[,] CreateRoads(string[,] world, GameObject[,] worldData)
    {
        int worldSize = world.GetLength(0);
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                // If we find a road
                if (world[x, y] == "R" || world[x, y] == "I")
                {
                    Vector2Int ActualPos;
                    if (world[x, y] == "I" || y - 1 > 0 && world[x, y - 1] == "R" && y + 1 < worldSize && world[x, y + 1] == "R" && x - 1 > 0 && world[x - 1, y] == "R" && x + 1 < worldSize && world[x + 1, y] == "R")
                    {
                        ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, intersectPrefab);
                        GameObject trp = GameObject.Instantiate(intersectPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                        trp.transform.Rotate(new Vector3(-90, 0, 0));

                        worldData[x, y] = trp;
                    }
                    // Dealing with T-Bones Intersect
                    else if (y - 1 > 0 && (world[x, y - 1] == "R" || world[x, y - 1] == "I") && y + 1 < worldSize && (world[x, y + 1] == "R" || world[x, y + 1] == "I"))
                    {
                        if (x - 1 > 0 && world[x - 1, y] == "R")
                        {
                            GameObject tbonePrefab = tBonePrefabs[random.Next(0, tBonePrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, tbonePrefab);
                            GameObject trp = GameObject.Instantiate(tbonePrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            trp.transform.Rotate(new Vector3(-90, 0, 270));

                            worldData[x, y] = trp;
                        }
                        else if (x + 1 < worldSize && world[x + 1, y] == "R")
                        {
                            GameObject tbonePrefab = tBonePrefabs[random.Next(0, tBonePrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, tbonePrefab);
                            GameObject trp = GameObject.Instantiate(tbonePrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            trp.transform.Rotate(new Vector3(-90, 0, 90));

                            worldData[x, y] = trp;
                        }
                        else
                        {
                            GameObject roadPrefab = roadPrefabs[random.Next(0, roadPrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, roadPrefab);
                            GameObject rp = GameObject.Instantiate(roadPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            rp.transform.Rotate(new Vector3(-90, 0, 90));

                            worldData[x, y] = rp;
                        }
                    }
                    else if (x - 1 > 0 && (world[x - 1, y] == "R" || world[x - 1, y] == "I") && x + 1 < worldSize && (world[x + 1, y] == "R" || world[x + 1, y] == "I"))
                    {
                        if (y - 1 > 0 && world[x, y - 1] == "R")
                        {
                            GameObject tbonePrefab = tBonePrefabs[random.Next(0, tBonePrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, tbonePrefab);
                            GameObject trp = GameObject.Instantiate(tbonePrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            trp.transform.Rotate(new Vector3(-90, 0, 180));

                            worldData[x, y] = trp;
                        }
                        else if (y + 1 < worldSize && world[x, y + 1] == "R")
                        {
                            GameObject tbonePrefab = tBonePrefabs[random.Next(0, tBonePrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, tbonePrefab);
                            GameObject trp = GameObject.Instantiate(tbonePrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            trp.transform.Rotate(new Vector3(-90, 0, 0));

                            worldData[x, y] = trp;
                        }
                        else
                        {
                            GameObject roadPrefab = roadPrefabs[random.Next(0, roadPrefabs.Count)];
                            ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, roadPrefab);
                            GameObject rp = GameObject.Instantiate(roadPrefab, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
                            rp.transform.Rotate(new Vector3(-90, 0, 0));

                            worldData[x, y] = rp;
                        }
                    }
                    // Dealing with Elbows
                    /*else if ()
                    {

                    }*/
                    // Dead ends
                    else if (x - 1 > 0 && world[x - 1, y] == "R" || x + 1 < worldSize && world[x + 1, y] == "R")
                    {

                    }
                    else if (y - 1 > 0 && world[x, y - 1] == "R" || y + 1 < worldSize && world[x, y + 1] == "R")
                    {

                    }
                }
            }
        }
        return worldData;

    }

    public Vector2Int ComputePosition(Vector2Int coordinates, int worldSize, GameObject road)
    {
        Renderer roadRenderer = road.GetComponent<Renderer>();
        float size = roadRenderer.bounds.size.x;
        int newX = (coordinates.x - worldSize / 2) - (int)(size / 2);
        int newY = (coordinates.y - worldSize / 2) - (int)(size / 2);

        return new Vector2Int(newX, newY);
    }
}
