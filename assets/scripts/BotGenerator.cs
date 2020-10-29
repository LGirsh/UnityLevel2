using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public class BotGenerator : MonoBehaviour
{
    private static XmlSerializer serializer;
    [SerializeField] private GameObject botPref;

    private string SavingPath;
    private int EnemyCounter;

    private float startTime;
    public float CountDown = 5f;

    private void Awake()
    {
        SavingPath = PlayerPrefs.GetString("Waypoint");
        serializer = new XmlSerializer(typeof(SVect3[]));
       
        int count = 5;
        while(count > EnemyCounter)
        {
            float x = Random.Range(-30, 30);
            float z = Random.Range(-30, 30);
            Vector3 pos = new Vector3(x, 5f, z);

            GameObject temp = Instantiate(botPref, pos,Quaternion.identity);
            Bot tBot = temp.GetComponent<Bot>();
            SVect3[] result;

            using(FileStream fs = new FileStream(SavingPath, FileMode.Open))
            {
                result = (SVect3[])serializer.Deserialize(fs);
            }
            foreach(Vector3 Pos in result)
            {
                tBot.WayPoints.Add(pos);
            }
            EnemyCounter++;
        }    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
