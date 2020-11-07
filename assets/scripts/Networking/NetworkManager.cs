using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class NetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{

    public static NetworkManager Instance = null;

    private static XmlSerializer serializer;

    [SerializeField] private GameObject botPref;
    [SerializeField] private GameObject player;
    [SerializeField] private MiniCamera MapCamera;

    private string SavingPath;
    private int EnemyCounter;
    private int MaxEnemyC = 5;

    private float Time;
    public float CountDown = 5f;
    public bool StartTimer;

    private IEnumerator SpawnEnemy(int count)
    {
        while (count > EnemyCounter)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            GameObject temp = Create("Prefabs/Enemy1");
            Bot tBot = temp.GetComponent<Bot>();
            NavMeshAgent agent = temp.GetComponent<NavMeshAgent>();
            agent.avoidancePriority = EnemyCounter;
            EnemyCounter++;

            SVect3[] result;

            using (FileStream fs = new FileStream(SavingPath, FileMode.Open))
            {
                result = (SVect3[])serializer.Deserialize(fs);
            }
            foreach (Vector3 Pos in result)
            {
                tBot.WayPoints.Add(Pos);
            }
        }
    }
    GameObject Create(string PrefabName)
    {
        float x = Random.Range(-30, 30);
        float z = Random.Range(-30, 30);
        Vector3 pos = new Vector3(x, 5f, z);
        GameObject temp = PhotonNetwork.Instantiate(PrefabName,pos, Quaternion.identity);
        return temp;
    }

    void StartGame()
    {
        if (!player)
        {
            player = Create("Prefabs/Player");
            MiniCamera.Player = player.transform;
            
        }

        if(PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
        {
            StartCoroutine(SpawnEnemy(MaxEnemyC));
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StartTimer = false;

    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartTimer = true;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SavingPath = PlayerPrefs.GetString("Waypoint");
        serializer = new XmlSerializer(typeof(SVect3[]));
       
        
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (StartTimer)
        {
            float timer = (float)PhotonNetwork.Time - Time;
            if(CountDown - timer <= 0)
            {
                StartGame();
                StartTimer = false;
            }
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartCoroutine(SpawnEnemy(MaxEnemyC - EnemyCounter));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(EnemyCounter);
        }
        else
        {
            EnemyCounter = (int)stream.ReceiveNext();
        }
    }
}
