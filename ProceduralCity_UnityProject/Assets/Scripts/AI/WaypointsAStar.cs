using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Node class encapsulating a Waypoint to create a navigation graph for the AStar Algorithm
/// </summary>
public class WaypointNode : IEquatable<WaypointNode>
{
    public Waypoint waypoint { get; set; }
    public WaypointNode Parent { get; set; }
    public float H { get; set; }
    public float F { get; set; }
    public float C { get; set; }

    public WaypointNode(Waypoint pos, float c = 0)
    {
        waypoint = pos;
        Parent = null;
        C = c;
    }

    public WaypointNode(Waypoint pos, WaypointNode parent, float c = 0)
    {
        waypoint = pos;
        Parent = parent;
        C = c;
    }

    public bool Equals(WaypointNode other)
    {
        return other.waypoint.transform.position == waypoint.transform.position;
    }
}

/// <summary>
/// A class to compute AStar in a waypoint graph
/// </summary>
public class WaypointsAStar
{
    public string Name { get; set; }

    public WaypointsAStar(string name)
    {
        Name = name;
    }

    public List<Waypoint> AStar(Waypoint start, Waypoint end)
    {
        WaypointNode startNode = new WaypointNode(start);
        WaypointNode endNode = new WaypointNode(end);
        List<WaypointNode> openSet = new List<WaypointNode>() { startNode };
        List<WaypointNode> closedSet = new List<WaypointNode>();

        int maxIter = 0; //A failsafe to avoid infinite loops due to lack of convergence

        while (!closedSet.Contains(endNode) && maxIter < 5000)
        {
            openSet = openSet.OrderBy(node => node.F).ToList();

            if (openSet.Count == 0) return null;
            WaypointNode current = openSet[0];

            // if the destination is found return the path
            if (current.waypoint == end)
                return Unpile(current);
            foreach (Waypoint n in current.waypoint.next)
            {
                if(n != null)
                {
                    //WaypointNode neigbhor = new WaypointNode(n, current, current.C + EuclideanDist(n.transform.position, current.Pos));
                    WaypointNode neigbhor = new WaypointNode(n, current, current.C + 1);
                    if (!closedSet.Contains(neigbhor) || !FindBest(neigbhor, openSet))
                    {
                        neigbhor.H = 1;//EuclideanDist(neigbhor.waypoint.transform.position, endNode.waypoint.transform.position);
                        neigbhor.F = neigbhor.H + neigbhor.C;
                        //Debug.Log(neigbhor.F);
                        openSet.Insert(0, neigbhor);
                    }
                }
            }
            openSet.Remove(current);
            closedSet.Add(current);
            maxIter++;
        }
        Debug.Log("TIME LIMIT EXCEDEED");
        return null;
    }

    bool FindBest(WaypointNode current, List<WaypointNode> openSet)
    {
        WaypointNode test = openSet.Find(x => x.Equals(current));
        if (test != null)
            if (test.F < current.F)
                return true;
        return false;
    }

    // Compute the euclidean distance between two Vector3
    float EuclideanDist(Vector3 w1, Vector3 w2)
    {
        return (float)Math.Sqrt(Math.Pow((w1.x - w2.x), 2) + Math.Pow((w1.z - w2.z), 2));
    }

    // Convert the last Node to the all path using the parenting relationship
    public List<Waypoint> Unpile(WaypointNode w)
    {
        List<Waypoint> result = new List<Waypoint>();
        while (w.Parent != null)
        {
            result.Add(w.waypoint);
            w = w.Parent;
        }
        return result;
    }
}
