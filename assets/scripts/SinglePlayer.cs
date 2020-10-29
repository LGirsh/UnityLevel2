using System;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public string PlayerName;
    public int PlayerHealth;
    public bool PlayerDead;
}


public class SinglePlayer : Unit
{
    private ISaveData _data;

    RaycastHit hit;
    Ray ray;
    Transform McamT;
    Transform target;
    private bool Grab;
    [SerializeField] LayerMask Layer;

    protected override void Awake()
    {
        base.Awake();
        Health = 100;
        Dead = false;

        // _data = new StreamData();
        PlayerData SinglePlayerData = new PlayerData
        {
            PlayerDead = Dead, PlayerHealth = Health, PlayerName = name
        };

        // PlayerPrefs.SetString("Name", SinglePlayerData.PlayerName);
        // PlayerPrefs.Save();

        // Debug.Log(PlayerPrefs.GetString("Name"));

        // PlayerPrefs.DeleteAll();

        // Debug.Log(PlayerPrefs.GetString("Name"));

        // _data.Save(SinglePlayerData);
        // PlayerData NewPlayer = _data.Load();

        // Debug.Log(NewPlayer.PlayerName);
        // Debug.Log(NewPlayer.PlayerHealth);
        // Debug.Log(NewPlayer.PlayerDead);

    }

    void Start()
    {
        McamT = Camera.main.transform;
    }


    void Update()
    {
        ray = new Ray(McamT.position,McamT.forward);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab = !Grab;
        }


        if (Grab)
        {
            if (Physics.Raycast(ray, out hit, 50,Layer))
            {
                if (hit.collider.tag == "Pickup")
                {
                    hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                    hit.transform.parent = McamT;
                }
            }            
        }
        else
        {
            if (hit.transform)
            {
                hit.transform.parent = null;
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.GetComponent<Rigidbody>().AddForce(Vector3.forward*0.5f, ForceMode.Impulse);
            }
           
            
        }
    }
}
