using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{/*
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * .5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, .05f);
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
                    waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        if(waypoint.previous != null)
        {
            Gizmos.color = Color.red;
            foreach(Waypoint w in waypoint.previous)
            {
                Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = w.transform.right * w.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offset, w.transform.position + offsetTo);
            }
        }
        if (waypoint.next != null)
        {
            Gizmos.color = Color.green;
            foreach (Waypoint w in waypoint.next)
            {
                Vector3 offset = waypoint.transform.right * -waypoint.width / 2f;
                Vector3 offsetTo = w.transform.right * -w.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offset, w.transform.position + offsetTo);
            }
        }
    }
*/
}
