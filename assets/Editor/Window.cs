using UnityEditor;
using UnityEngine;

public class Window : EditorWindow
{
    public GameObject botPref;
    public GameObject main = null;
    public int objCounter;
    public float radius = 20;
    private string containerName = "MainForBotGenerator"; // My code

    [MenuItem("Инструменты/ Создание префабов/ Генератор ботов")]
    public static void ShowWindowGenerator()
    {
        GetWindow(typeof(Window), false, "Генератор ботов");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Настройка", EditorStyles.boldLabel);

        botPref = EditorGUILayout.ObjectField("Префаб бота", botPref, typeof(GameObject), true)as GameObject;
        objCounter = EditorGUILayout.IntSlider("Количество префабов", objCounter, 3, 200);
        radius = EditorGUILayout.Slider("Радиус", radius,10,100);

        if (GUILayout.Button("Создать")) 
        {
            if (botPref)
            {
                // GameObject main = new GameObject("Main");
                if(main) //my code
                    return;// my code

                main = new GameObject(containerName);

                for (int i = 0; i < objCounter; i++)
                {
                    float angle = i * Mathf.PI * 2 / objCounter;
                    Vector3 pos = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius);
                    GameObject temp = Instantiate(botPref, pos, Quaternion.identity);
                    temp.transform.parent = main.transform;
                    temp.name = "Bot (" + i + ")";
                }
            }
        }

        // My code >>>>
        if (GUILayout.Button("Выключить") && main)
        {
            for (int i = 0; i < main.transform.childCount; i++)
            { 
                Transform o = main.transform.GetChild(i);
                o.gameObject.SetActive(false);
            }
        }

        if (GUILayout.Button("Удалить") && main)
        {
            DestroyImmediate(main);
            main = null;
        }

        // My code <<<<


    }

}
