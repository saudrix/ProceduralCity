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
        Waypoint RightIn = rl.transform.Find("RightIn").gameObject.GetComponent<Waypoint>();
        Waypoint RightOut = rl.transform.Find("RightOut").gameObject.GetComponent<Waypoint>();

        Waypoint LeftIn = rr.transform.Find("LeftIn").gameObject.GetComponent<Waypoint>();
        Waypoint LeftOut = rr.transform.Find("LeftOut").gameObject.GetComponent<Waypoint>();

        ConnectWaypoint(RightOut, LeftIn);
        ConnectWaypoint(LeftOut, RightIn);
    }

    public void ConnectRoadDown(GameObject tr, GameObject dr)
    {
        Waypoint BottomIn = tr.transform.Find("BottomIn").gameObject.GetComponent<Waypoint>();
        Waypoint BottomOut = tr.transform.Find("BottomOut").gameObject.GetComponent<Waypoint>();

        Waypoint TopIn = dr.transform.Find("TopIn").gameObject.GetComponent<Waypoint>();
        Waypoint TopOut = dr.transform.Find("TopOut").gameObject.GetComponent<Waypoint>();

        ConnectWaypoint(TopOut, BottomIn);
        ConnectWaypoint(BottomOut, TopIn);
    }

    public void ConnectWaypoint(Waypoint first, Waypoint second)
    {
        first.next.Add(second);
        second.previous.Add(first);
    }
}
