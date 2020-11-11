using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{
   public override void OnInspectorGUI()
   {
      AIWaypointNetwork network = (AIWaypointNetwork) target;

      network.DisplayMode = (PathDisplayMode) EditorGUILayout.EnumPopup("Display Mode", network.DisplayMode);

      if (network.DisplayMode == PathDisplayMode.Paths)
      {
         network.UIStart = EditorGUILayout.IntSlider("WP Start", network.UIStart, 0, network.Waypoints.Count - 1);
         network.UIEnd = EditorGUILayout.IntSlider("WP End", network.UIEnd, 0, network.Waypoints.Count - 1);
      }

      DrawDefaultInspector();
   }

   void OnSceneGUI()
   {
      AIWaypointNetwork network = (AIWaypointNetwork) target;
      Vector3[] linePoints = new Vector3[ network.Waypoints.Count + 1];

      for (int i = 0; i < network.Waypoints.Count; i++)
      {
         if (network.Waypoints[i] != null)
         {
            Handles.Label( network.Waypoints[i].position, "Waypoint " + i.ToString());
            linePoints[i] = network.Waypoints[i].position;
         }
      }

      // This store the first waypoint to close the path/loop
      linePoints[network.Waypoints.Count] = network.Waypoints[0].position;



      switch (network.DisplayMode)
      {
         case PathDisplayMode.Connections:
         {
            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePoints);
            break;
         }

         case PathDisplayMode.Paths:
         {
            NavMeshPath path = new NavMeshPath();
            Vector3 from = network.Waypoints[network.UIStart].position;
            Vector3 to = network.Waypoints[network.UIEnd].position;

            NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);

            Handles.color = Color.yellow;
            Handles.DrawPolyLine(path.corners);

            break;
         }
      }



   }
}
