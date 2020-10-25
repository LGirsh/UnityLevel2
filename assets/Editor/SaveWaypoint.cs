using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

[CustomEditor(typeof(WayPointPath))]
public class SaveWayPoint : Editor
{
    private static XmlSerializer serializer;
    public List<SVect3> SavingNodes = new List<SVect3>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializer = new XmlSerializer(typeof(SVect3[]));

        WayPointPath Wpath = (WayPointPath)target;

        if (GUILayout.Button("Сохранить"))
        {
            if(Wpath.nodes.Count > 0)
            {
                foreach(Transform T in Wpath.nodes)
                {
                    if (!SavingNodes.Contains(T.position))
                    {
                        SavingNodes.Add(T.position);
                    }
                }
            }

            PlayerPrefs.SetString("Waypoint", Wpath.SavingPath);
            PlayerPrefs.Save();

        }

        using (FileStream fs = new FileStream(Wpath.SavingPath, FileMode.Create))
        {
            serializer.Serialize(fs, SavingNodes.ToArray());
        }
    }

}
