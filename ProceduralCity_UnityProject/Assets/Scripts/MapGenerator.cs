using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapGenerator
{
    [Range(2, 7)]
    public float densityFrenquency;

    public void SetDensity(SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);
        for (int x = 0; x < worldSize; x++)
            for (int y = 0; y < worldSize; y++)
            {
                float sample = Mathf.PerlinNoise((((float)x) / worldSize) * densityFrenquency, (((float)y) / worldSize) * densityFrenquency);
                worldData[x, y].density = sample;
            }
    }

    public void SetMainRoads(SimData[,] worldData)
    {

    }

    // Method that update the world to add buildings
    public void AddBuildingLog(SimData[,] world)
    {
        int worldSize = world.GetLength(0);
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                if (Utils.ScanNeighborhood(new Vector2Int(x, y), 1, "R", world).Count > 0 && world[x, y].repr == null)
                    world[x, y].repr = "H";
            }
        }
    }
}
