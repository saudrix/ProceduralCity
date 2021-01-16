using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapGenerator
{
    #region UserFields
    [Range(2, 7)]
    public float densityFrenquency;
    [Range(20, 200)]
    [SerializeField]
    public int maxSample;
    [Range(10, 100)]
    [SerializeField]
    public int maxLowSample;
    #endregion

    public void GenerateMap(SimData[,] worldData)
    {
        SetDensity(worldData);
        SetMainRoads(worldData);
        AddBuildingLog(worldData);
        DiscriminateRoad(worldData);
    }

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
        // Creating DownTown
        CreateIntersection(worldData, .6f, maxSample, 40);
        // Creating Suburbs
        CreateIntersection(worldData, .4f, maxLowSample, 60, false);
    }

    List<Vector2Int> CreateIntersection(SimData[,] worldData, float densityTarget, int sampleCount, int maxRLenght, bool sampleAbove = true)
    {
        List<Vector2Int> extremities = new List<Vector2Int>();
        int side = sampleAbove ? 1 : -1;
        List<Vector2Int> intersections = SampleIntersection(worldData, densityTarget, sampleCount, side);
        foreach (Vector2Int pos in intersections)
            extremities.AddRange(ExtendsIntersect(worldData, pos, maxRLenght));
        Debug.Log(intersections.Count);
        return extremities;
    }

    List<Vector2Int> SampleIntersection(SimData[,] worldData, float threshold, int sampleCount, int side = 1)
    {
        List<Vector2Int> samples = new List<Vector2Int>();
        int nbIntersect = 0;
        int interDist = 2;
        int worldSize = worldData.GetLength(0);
        int safe = 1000;

        while (nbIntersect < sampleCount && safe > 0)
        {
            int rnX = UnityEngine.Random.Range(0, worldSize);
            int rnY = UnityEngine.Random.Range(0, worldSize);
            if (side * worldData[rnX, rnY].density > side * threshold)
            {
                //Debug.Log("Found density Potentiel Candidate");
                if (Utils.ScanNeighborhood(new Vector2Int(rnX, rnY), interDist, "I", worldData).Count == 0)
                {
                    //Debug.Log("Nothing near the candidate");
                    if (ColumnClear(rnX - 1, rnY, worldData) && ColumnClear(rnX + 1, rnY, worldData) && LineClear(rnY - 1, rnX, worldData) && LineClear(rnY + 1, rnX, worldData))
                    {
                        //Debug.Log("Lines & Columns Cleared For addition");
                        nbIntersect++;
                        worldData[rnX, rnY].repr = "I";
                        samples.Add(new Vector2Int(rnX, rnY));
                    }
                }
            }
            safe--;
        }
        return samples;
    }

    List<Vector2Int> ExtendsIntersect(SimData[,] worldData, Vector2Int inter, int maxRlength)
    {
        List<Vector2Int> ext = new List<Vector2Int>();
        int minRlength = 10;
        float densityAtPoint = worldData[inter[0], inter[1]].density;
        for (int i = 0; i < 4; i++)
        {
            int length = UnityEngine.Random.Range(minRlength, (int)(densityAtPoint * maxRlength));
            int dir = i % 2 == 0 ? 0 : 1;
            int sign = i < 2 ? -1 : 1;

            for (int id = 1; id < length; id++)
            {
                Vector2Int pos = inter;
                pos[dir] += id * sign;
                if (pos[dir] > 0 && pos[dir] < worldData.GetLength(dir))
                    if (worldData[pos[0], pos[1]].repr != "I")
                    {
                        int invDir = Math.Abs(1 - dir);
                        Vector2Int left = pos;
                        left[invDir] -= 1;
                        Vector2Int right = pos;
                        right[invDir] += 1;
                        if (left[invDir] > 0 && worldData[left[0], left[1]].repr == "R") break;
                        if (right[invDir] < worldData.GetLength(0) && worldData[right[0], right[1]].repr == "R") break;
                        worldData[pos[0], pos[1]].repr = "R";
                    }
            }
        }
        return ext;
    }

    bool ColumnClear(int x, int y, SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);
        if (x < 0 || x > worldData.GetLength(0) - 1) return true;
        int start = Math.Max(0, y - 20);
        int end = Math.Min(y + 20, worldSize);
        for (int i = start; i < end; i++)
        {
            if (worldData[x, i].repr != null) return false;
        }
        return true;
    }

    bool LineClear(int y, int x, SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);
        if (y < 0 || y > worldData.GetLength(1) - 1) return false;
        int start = Math.Max(0, x - 20);
        int end = Math.Min(x + 20, worldSize);
        for (int i = start; i < end; i++)
        {
            if (worldData[i, y].repr != null) return false;
        }
        return true;
    }

    // Method that update the world to add buildings
    public void AddBuildingLog(SimData[,] world)
    {
        int worldSize = world.GetLength(0);
        for (int y = 0; y < worldSize; y++)
            for (int x = 0; x < worldSize; x++)
            {
                if (Utils.ScanNeighborhood(new Vector2Int(x, y), 1, "R", world).Count > 0 && world[x, y].repr == null)
                    world[x, y].repr = "H";
            }
    }

    public void DiscriminateRoad(SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);
        string[,] updatedRoad = new string[worldSize,worldSize];

        List<string> targetChar = new List<string>() { "R", "I" };
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                if (worldData[x, y].repr == "I")
                    updatedRoad[x, y] = "I";
                
                else if(worldData[x,y].repr == "R")
                {
                    // Adding 4ways crossroads
                    if (y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr) && y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr)
                        && x - 1 >= 0 && targetChar.Contains(worldData[x - 1, y].repr) && x + 1 < worldSize && targetChar.Contains(worldData[x + 1, y].repr))
                        updatedRoad[x, y] = "I";

                    // T1 && T3 Verticals T Junctions and Vertical straights
                    else if ((y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr)) && (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr)))
                    {
                        if (x + 1 < worldSize && targetChar.Contains(worldData[x + 1, y].repr))
                            updatedRoad[x, y] = "T1";
                        else if (x - 1 >= 0 && targetChar.Contains(worldData[x - 1, y].repr))
                            updatedRoad[x, y] = "T3";
                        else updatedRoad[x, y] = "SV";
                    }

                    // T2 && T4 Horizontal T Junctions and Horizontal straights
                    else if ((x - 1 >= 0 && targetChar.Contains(worldData[x - 1, y].repr)) && (x + 1 < worldSize && targetChar.Contains(worldData[x + 1, y].repr)))
                    {
                        if (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr))
                            updatedRoad[x, y] = "T4";
                        else if (y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr))
                            updatedRoad[x, y] = "T2";
                        else updatedRoad[x, y] = "SH";
                    }
                    // Adding elbows differenciation
                    else if (x + 1 < worldSize && targetChar.Contains(worldData[x + 1, y].repr))
                    {
                        if (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr))
                            updatedRoad[x, y] = "C1";
                        else if (y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr))
                            updatedRoad[x, y] = "C4";
                        else updatedRoad[x, y] = "E1"; // If not its a dead end
                    }
                    else if (x - 1 >= 0 && targetChar.Contains(worldData[x - 1, y].repr))
                    {
                        if (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr))
                            updatedRoad[x, y] = "C2";
                        else if (y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr))
                            updatedRoad[x, y] = "C3";
                        else updatedRoad[x, y] = "E3"; // If not its a dead end
                    }
                    else if (y - 1 >= 0 && targetChar.Contains(worldData[x, y - 1].repr))
                        updatedRoad[x, y] = "E2";
                    else if (y + 1 < worldSize && targetChar.Contains(worldData[x, y + 1].repr))
                        updatedRoad[x, y] = "E4";
                }
            }
        }
        for (int y = 0; y < worldSize; y++)
            for (int x = 0; x < worldSize; x++)
                if (updatedRoad[x, y] != null) worldData[x, y].repr = updatedRoad[x, y];
    }
}
