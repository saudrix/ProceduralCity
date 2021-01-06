using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : IEquatable<Node>
{
    public Vector2Int Pos { get; set; }
    public Node Parent { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public int C { get; set; }

    public Node(Vector2Int pos, int c = 0)
    {
        Pos = pos;
        Parent = null;
        C = c;
    }

    public Node(Vector2Int pos, Node parent, int c = 0)
    {
        Pos = pos;
        Parent = parent;
        C = c;
    }

    public bool Equals(Node other)
    {
        return other.Pos == this.Pos;
    }
}

public class Astar
{
    string[,] world { get; set; }
    int worldSize { get; set; }

    public Astar(string[,] world)
    {
        this.world = world;
        worldSize = world.GetLength(0);
    }

    List<Vector2Int> AStar(Vector2Int start, Vector2Int end)
    {
        Debug.Log("start: " + start);
        Debug.Log("end: " + end);
        Node startNode = new Node(start);
        Node endNode = new Node(end);
        List<Node> openSet = new List<Node>() { startNode };
        List<Node> closedSet = new List<Node>();

        string[,] data = (string[,])world.Clone();
        int maxIter = 0;

        while (maxIter < 500 && !closedSet.Contains(endNode))
        {
            openSet = openSet.OrderBy(name => name.F).ToList();
            Node current = openSet[0];
            Debug.Log("Current Node = " + current.Pos);
            Debug.Log("Score =" + current.F);
            if (current.Pos == end)
                return Unpile(current);
            foreach (Vector2Int n in AllowedNeighbors(current.Pos))
            {
                Node neigbhor = new Node(n, current, current.C + 1);
                if (!closedSet.Contains(neigbhor) || !FindBest(neigbhor, openSet))
                {
                    neigbhor.H = ManhattanDist(neigbhor.Pos, endNode.Pos);
                    neigbhor.F = neigbhor.H + neigbhor.C;
                    //Debug.Log(neigbhor.F);
                    openSet.Insert(0, neigbhor);
                }
            }
            openSet.Remove(current);
            closedSet.Add(current);
            maxIter++;
        }
        Debug.Log("TIME LIMIT EXCEDEED");
        return null;
    }

    bool FindBest(Node current, List<Node> openSet)
    {
        Node test = openSet.Find(x => x.Equals(current));
        if (test != null)
            if (test.F < current.F)
                return true;
        return false;
    }

    List<Vector2Int> Get4Neigb(Vector2Int pos)
    {
        List<Vector2Int> neihgbors = new List<Vector2Int>();
        if (pos.x - 1 > 0) neihgbors.Add(new Vector2Int(pos.x - 1, pos.y));
        if (pos.x + 1 < worldSize) neihgbors.Add(new Vector2Int(pos.x + 1, pos.y));
        if (pos.y - 1 > 0) neihgbors.Add(new Vector2Int(pos.x, pos.y - 1));
        if (pos.y + 1 < worldSize) neihgbors.Add(new Vector2Int(pos.x, pos.y + 1));
        return neihgbors;
    }

    int ManhattanDist(Vector2Int n1, Vector2Int n2)
    {
        return Math.Abs(n2.x - n1.x) + Math.Abs(n2.y - n1.y);
    }

    List<Vector2Int> Unpile(Node n)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        while (n.Parent != null)
        {
            result.Add(n.Pos);
            n = n.Parent;
        }
        return result;
    }

    List<Vector2Int> AllowedNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neigbhors = new List<Vector2Int>();
        List<Vector2Int> inter = Get4Neigb(pos);
        foreach (Vector2Int i in inter)
        {
            List<Vector2Int> INeigb = ScanNeighborhood(i, 1, new List<string>() { "R", "I" });
            if (INeigb.Count < 2)
                neigbhors.Add(i);
        }
        //Debug.Log("ALLOWED NEIHGBORS COUNT = " + neigbhors.Count);
        return neigbhors;
    }

    List<Vector2Int> ScanNeighborhood(Vector2Int pos, int dist, List<string> vals)
    {
        List<Vector2Int> inter = new List<Vector2Int>();
        foreach (Vector2Int vec in GetNNeighbours(pos, dist))
            if (vals.Contains(world[vec.x, vec.y])) inter.Add(vec);
        return inter;
    }

    List<Vector2Int> GetNNeighbours(Vector2Int pos, int dist)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        for (int i = -dist; i <= dist; i++)
        {
            if (pos.x + i > 0 && pos.x + i < worldSize)
            {
                if (pos.y - dist > 0) neighbours.Add(new Vector2Int(pos.x + i, pos.y - dist));
                if (pos.y + dist < 0) neighbours.Add(new Vector2Int(pos.x + i, pos.y + dist));
            }
        }
        for (int j = (-dist) + 1; j <= dist - 1; j++)
        {
            if (pos.y + j > 0 && pos.y + j < worldSize)
            {
                if (pos.x - dist > 0) neighbours.Add(new Vector2Int(pos.x - dist, pos.y + j));
                if (pos.x + dist < worldSize) neighbours.Add(new Vector2Int(pos.x + dist, pos.y + j));
            }
        }
        return neighbours;
    }
}
