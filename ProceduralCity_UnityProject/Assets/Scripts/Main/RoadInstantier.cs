using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoadInstantier
{
    public List<GameObject> I_roads;

    public List<GameObject> T1_roads;
    public List<GameObject> T2_roads;
    public List<GameObject> T3_roads;
    public List<GameObject> T4_roads;

    public List<GameObject> SV_roads;
    public List<GameObject> SH_roads;

    public List<GameObject> C1_roads;
    public List<GameObject> C2_roads;
    public List<GameObject> C3_roads;
    public List<GameObject> C4_roads;

    public List<GameObject> E1_roads;
    public List<GameObject> E2_roads;
    public List<GameObject> E3_roads;
    public List<GameObject> E4_roads;


    public GameObject roadParent;

    System.Random random = new System.Random();

    public void CreateRoads(SimData[,] worldData)
    {
        int worldSize = worldData.GetLength(0);

        List<GameObject> roads = new List<GameObject>();

        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                switch (worldData[x, y].repr)
                {
                    case "I":
                        GameObject I_road_prefab = I_roads[random.Next(0, I_roads.Count)];
                        roads.Add(AddRoad(I_road_prefab, x, y, 0, worldSize, worldData, roadParent));
                        break;

                    case "T1":
                        GameObject T1_road_prefab = T1_roads[random.Next(0, T1_roads.Count)];
                        roads.Add(AddRoad(T1_road_prefab, x, y, 180, worldSize, worldData, roadParent, -90));
                        break;

                    case "T2":
                        GameObject T2_road_prefab = T2_roads[random.Next(0, T2_roads.Count)];
                        roads.Add(AddRoad(T2_road_prefab, x, y, 180, worldSize, worldData, roadParent));
                        break;

                    case "T3":
                        GameObject T3_road_prefab = T3_roads[random.Next(0, T3_roads.Count)];
                        roads.Add(AddRoad(T3_road_prefab, x, y, 0, worldSize, worldData, roadParent, -90));
                        break;

                    case "T4":
                        GameObject T4_road_prefab = T4_roads[random.Next(0, T4_roads.Count)];
                        roads.Add(AddRoad(T4_road_prefab, x, y, 0, worldSize, worldData, roadParent));
                        break;

                    case "SV":
                        GameObject SV_road_prefab = SV_roads[random.Next(0, SV_roads.Count)];
                        roads.Add(AddRoad(SV_road_prefab, x, y, 90, worldSize, worldData, roadParent));
                        break;

                    case "SH":
                        GameObject SH_road_prefab = SH_roads[random.Next(0, SH_roads.Count)];
                        roads.Add(AddRoad(SH_road_prefab,x,y,0,worldSize,worldData,roadParent));
                        break;

                    case "E1":
                        GameObject E1_road_prefab = E1_roads[random.Next(0, E1_roads.Count)];
                        roads.Add(AddRoad(E1_road_prefab, x, y, 0, worldSize, worldData, roadParent));
                        break;
                    case "E2":
                        GameObject E2_road_prefab = E2_roads[random.Next(0, E2_roads.Count)];
                        roads.Add(AddRoad(E2_road_prefab, x, y, 90, worldSize, worldData, roadParent));
                        break;
                    case "E3":
                        GameObject E3_road_prefab = E3_roads[random.Next(0, E3_roads.Count)];
                        roads.Add(AddRoad(E3_road_prefab, x, y, 180, worldSize, worldData, roadParent));
                        break;
                    case "E4":
                        GameObject E4_road_prefab = E4_roads[random.Next(0, E4_roads.Count)];
                        roads.Add(AddRoad(E4_road_prefab, x, y, 270, worldSize, worldData, roadParent));
                        break;
                }
            }
        }

        roadParent.SetActive(true);
        foreach(GameObject r in roads) { r.SetActive(false); r.SetActive(true); }
        StaticBatchingUtility.Combine(roads.ToArray(), roadParent);
    }

    public Vector2Int ComputePosition(Vector2Int coordinates, int worldSize, GameObject road)
    {
        Renderer roadRenderer = road.GetComponent<Renderer>();
        float size = roadRenderer.bounds.size.x;
        int newX = (coordinates.x - worldSize / 2) - (int)(size / 2);
        int newY = (coordinates.y - worldSize / 2) - (int)(size / 2);

        return new Vector2Int(newX, newY);
    }

    public GameObject AddRoad(GameObject prefabRef, int x, int y, int rotation, int worldSize, SimData[,] worldData, GameObject parent = null, int zRot = 0)
    {
        Vector2Int ActualPos = ComputePosition(new Vector2Int(x, y), worldSize, prefabRef);
        GameObject goRef = GameObject.Instantiate(prefabRef, new Vector3(ActualPos.x, 0, ActualPos.y), Quaternion.identity);
        goRef.transform.Rotate(new Vector3(-90, rotation, zRot));
        goRef.transform.parent = parent.transform;
        worldData[x, y].model = goRef;
        goRef.isStatic = true;
        goRef.tag = "roads";

        return goRef;
    }
}
