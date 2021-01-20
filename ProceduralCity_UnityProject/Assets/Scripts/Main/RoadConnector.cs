using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    
[Serializable]
public class RoadConnector
{
    public void ConnectRoads(SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if(worldData[x,y].model != null && worldData[x,y].model.tag == "roads")
                {
                    if(x+1 < worldSize && worldData[x+1, y].model != null && worldData[x +1 ,y].model.tag == "roads")
                    {
                        ConnectRoadRight(worldData[x, y].model, worldData[x + 1, y].model);
                    }
                    if(y+1 < worldSize && worldData[x,y+1].model != null && worldData[x,y+1].model.tag == "roads")
                    {
                        ConnectRoadDown(worldData[x, y].model, worldData[x, y + 1].model);
                    }
                }
            }
        }
    }

    public void ConnectRoadRight(GameObject rl, GameObject rr)
    {
        // Connecting the car graph
        Waypoint RightIn = rl.transform.Find("RightIn").gameObject.GetComponent<Waypoint>();
        Waypoint RightOut = rl.transform.Find("RightOut").gameObject.GetComponent<Waypoint>();

        Waypoint LeftIn = rr.transform.Find("LeftIn").gameObject.GetComponent<Waypoint>();
        Waypoint LeftOut = rr.transform.Find("LeftOut").gameObject.GetComponent<Waypoint>();

        ConnectWaypoint(RightOut, LeftIn);
        ConnectWaypoint(LeftOut, RightIn);

        // Connecting the pedestrian graph
        RightIn = rl.transform.Find("RightIn_P").gameObject.GetComponent<Waypoint>();
        RightOut = rl.transform.Find("RightOut_P").gameObject.GetComponent<Waypoint>();

        LeftIn = rr.transform.Find("LeftIn_P").gameObject.GetComponent<Waypoint>();
        LeftOut = rr.transform.Find("LeftOut_P").gameObject.GetComponent<Waypoint>();

        ConnectWaypointBidirectionnal(RightOut, LeftIn);
        ConnectWaypointBidirectionnal(LeftOut, RightIn);
    }

    public void ConnectRoadDown(GameObject tr, GameObject dr)
    {
        // Connecting the car graph
        Waypoint BottomIn = tr.transform.Find("BottomIn").gameObject.GetComponent<Waypoint>();
        Waypoint BottomOut = tr.transform.Find("BottomOut").gameObject.GetComponent<Waypoint>();

        Waypoint TopIn = dr.transform.Find("TopIn").gameObject.GetComponent<Waypoint>();
        Waypoint TopOut = dr.transform.Find("TopOut").gameObject.GetComponent<Waypoint>();

        ConnectWaypoint(TopOut, BottomIn);
        ConnectWaypoint(BottomOut, TopIn);

        // Connecting the pedestrian graph
        BottomIn = tr.transform.Find("BottomIn_P").gameObject.GetComponent<Waypoint>();
        BottomOut = tr.transform.Find("BottomOut_P").gameObject.GetComponent<Waypoint>();

        TopIn = dr.transform.Find("TopIn_P").gameObject.GetComponent<Waypoint>();
        TopOut = dr.transform.Find("TopOut_P").gameObject.GetComponent<Waypoint>();

        ConnectWaypointBidirectionnal(TopOut, BottomIn);
        ConnectWaypointBidirectionnal(BottomOut, TopIn);
    }

    public void ConnectWaypointBidirectionnal(Waypoint w1, Waypoint w2)
    {
        w1.next.Add(w2);
        w1.previous.Add(w2);
        w2.next.Add(w1);
        w2.previous.Add(w1);
    }

    public void ConnectWaypoint(Waypoint first, Waypoint second)
    {
        first.next.Add(second);
        second.previous.Add(first);
    }
}
