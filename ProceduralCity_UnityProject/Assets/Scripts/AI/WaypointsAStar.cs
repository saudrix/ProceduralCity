using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointsAStar
{
    public class WaypointNode : IEquatable<WaypointNode>
    {
        public Vector3 Pos { get { return waypoint.transform.position; } }

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

    public class AstarWaypoint
    {
        public string Name { get; set; }

        public AstarWaypoint(string name)
        {
            Name = name;
        }

        public List<Waypoint> AStar(Waypoint start, Waypoint end)
        {
            WaypointNode startNode = new WaypointNode(start);
            WaypointNode endNode = new WaypointNode(end);
            List<WaypointNode> openSet = new List<WaypointNode>() { startNode };
            List<WaypointNode> closedSet = new List<WaypointNode>();

            int maxIter = 0;

            while (maxIter < 500 && !closedSet.Contains(endNode))
            {
                openSet = openSet.OrderBy(node => node.F).ToList();
                WaypointNode current = openSet[0];

                if (current.waypoint == end)
                    return Unpile(current);
                foreach (Waypoint n in current.waypoint.next)
                {
                    WaypointNode neigbhor = new WaypointNode(n, current, current.C + EuclideanDist(n.transform.position, current.Pos));
                    if (!closedSet.Contains(neigbhor) || !FindBest(neigbhor, openSet))
                    {
                        neigbhor.H = EuclideanDist(neigbhor.Pos, endNode.Pos);
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

        bool FindBest(WaypointNode current, List<WaypointNode> openSet)
        {
            WaypointNode test = openSet.Find(x => x.Equals(current));
            if (test != null)
                if (test.F < current.F)
                    return true;
            return false;
        }

        float EuclideanDist(Vector3 w1, Vector3 w2)
        {
            return (float)Math.Sqrt(Math.Pow((w1.x * w2.x), 2) + Math.Pow((w1.y * w2.y), 2));
        }

        List<Waypoint> Unpile(WaypointNode w)
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
}
