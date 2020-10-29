using UnityEditor;
using UnityEngine;

public class Window : EditorWindow
{
    public GameObject botPref;
    private string containerForBotsName = "containerForBotGenerator"; // My code
    public GameObject containerForBots = null;

    public int objCounter;
    public float radius = 20;

    private string containerForObjectsName = "containerForObjectsGenerator"; // My code
    public GameObject containerForObjects = null;

    [MenuItem("Инструменты/ Создание префабов/ Генератор")]
    public static void ShowWindowGenerator()
    {
        GetWindow(typeof(Window), false, "Генератор");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Настройка", EditorStyles.boldLabel);

        botPref = EditorGUILayout.ObjectField("Префаб бота", botPref, typeof(GameObject), true)as GameObject;
        objCounter = EditorGUILayout.IntSlider("Количество префабов", objCounter, 3, 200);
        radius = EditorGUILayout.Slider("Радиус", radius,10,100);

        if (GUILayout.Button("Создать ботов")) 
        {
            if (botPref)
            {
                // GameObject main = new GameObject("Main");
                if(containerForBots) //my code
                    return;// my code

                containerForBots = new GameObject(containerForBotsName);

                for (int i = 0; i < objCounter; i++)
                {
                    float angle = i * Mathf.PI * 2 / objCounter;
                    Vector3 pos = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius);
                    GameObject temp = Instantiate(botPref, pos, Quaternion.identity);
                    temp.transform.parent = containerForBots.transform;
                    temp.name = "Bot (" + i + ")";
                }
            }
        }

        // My code >>>>
        if (GUILayout.Button("Выключить ботов") && containerForBots)
        {
            for (int i = 0; i < containerForBots.transform.childCount; i++)
            { 
                Transform o = containerForBots.transform.GetChild(i);
                o.gameObject.SetActive(false);
            }
        }

        if(GUILayout.Button("Удалить ботов") && containerForBots)
        { 
            DestroyImmediate(containerForBots);
            containerForBots = null;
        }

        // My code <<<<

        if (GUILayout.Button("Создать предметы"))
        {
            if (containerForObjects) //my code
                return;// my code

            containerForObjects = new GameObject(containerForObjectsName);

            for (int i = 0; i < 5 ; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-30,30), 1.34f, Random.Range(-30, 30));
                GameObject temp = Instantiate(
                    Resources.Load("Pickup", typeof(GameObject)), 
                    pos, 
                    Quaternion.identity) as GameObject;

                temp.transform.parent = containerForObjects.transform;
                temp.name = "MyObject (" + i + ")";
            }
        }

        if (GUILayout.Button("Удалить предметы") && containerForObjects)
        {
            DestroyImmediate(containerForObjects);
            containerForObjects = null;
        }
    }
}

