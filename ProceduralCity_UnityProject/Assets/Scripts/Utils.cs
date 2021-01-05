using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct SimData
{
    public float density { get; set; }
    public string repr { get; set; }
    public GameObject model { get; set; }
}

public static class Utils
{
    #region Scanning
    public static List<Vector2Int> ScanNeighborhood(Vector2Int pos, int dist, string val, SimData[,] world)
    {
        List<Vector2Int> inter = new List<Vector2Int>();
        foreach (Vector2Int vec in GetNNeighbours(pos, dist, world.GetLength(0)))
            if (world[vec.x, vec.y].repr == val) inter.Add(vec);
        return inter;
    }

    public static List<Vector2Int> ScanNeighborhood(Vector2Int pos, int dist, List<string> vals, SimData[,] world)
    {
        List<Vector2Int> inter = new List<Vector2Int>();
        foreach (Vector2Int vec in GetNNeighbours(pos, dist, world.GetLength(0)))
            if (vals.Contains(world[vec.x, vec.y].repr)) inter.Add(vec);
        return inter;
    }

    public static List<Vector2Int> ScanNeighborhood(Vector2Int pos, int minDist, int maxDist, string val, SimData[,] world)
    {
        List<Vector2Int> inter = new List<Vector2Int>();
        for (int i = minDist; i <= maxDist; i++)
        {
            foreach (Vector2Int vec in GetNNeighbours(pos, i, world.GetLength(0)))
                if (world[vec.x, vec.y].repr == val) inter.Add(vec);
        }
        return inter;
    }

    public static List<Vector2Int> ScanNeighborhood(Vector2Int pos, int minDist, int maxDist, List<string> vals, SimData[,] world)
    {
        List<Vector2Int> inter = new List<Vector2Int>();
        for (int i = minDist; i <= maxDist; i++)
        {
            foreach (Vector2Int vec in GetNNeighbours(pos, i, world.GetLength(0)))
                if (vals.Contains(world[vec.x, vec.y].repr)) inter.Add(vec);
        }
        return inter;
    }
    #endregion

    public static List<Vector2Int> GetNNeighbours(Vector2Int pos, int dist, int worldSize)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        for (int i = -dist; i <= dist; i++)
        {
            if (pos.x + i > 0 && pos.x + i < worldSize)
            {
                if (pos.y - dist > 0) neighbours.Add(new Vector2Int(pos.x + i, pos.y - dist));
                if (pos.y + dist < worldSize) neighbours.Add(new Vector2Int(pos.x + i, pos.y + dist));
            }
        }
        for (int j = (-dist); j <= dist; j++)
        {
            if (pos.y + j > 0 && pos.y + j < worldSize)
            {
                if (pos.x - dist > 0) neighbours.Add(new Vector2Int(pos.x - dist, pos.y + j));
                if (pos.x + dist < worldSize) neighbours.Add(new Vector2Int(pos.x + dist, pos.y + j));
            }
        }
        return neighbours;
    }

    public static int ManhattanDist(Vector2Int n1, Vector2Int n2)
    {
        return Math.Abs(n2.x - n1.x) + Math.Abs(n2.y - n1.y);
    }

    /*List<Vector2Int> FindClosestIntersections(Vector2Int pos, int offset, int nbNeighbours, string val)
    {
        List<Vector2Int> closestIntersects = new List<Vector2Int>();
        int nbDist = offset;
        while (closestIntersects.Count < nbNeighbours)
        {
            closestIntersects.AddRange(Utils.ScanNeighborhood(pos, nbDist, val, world));
            nbDist++;
        }
        return closestIntersects;
    }*/
}
