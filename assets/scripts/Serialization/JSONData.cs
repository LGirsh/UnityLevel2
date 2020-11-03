using System.IO;
using UnityEngine;

public class JSONData : ISaveData
{
    string savePath = Path.Combine(UnityEngine.Application.dataPath, "JSONData.json");

    public void Save(PlayerData player)
    {
        string FileJson = JsonUtility.ToJson(player);
        File.WriteAllText(savePath, FileJson);

    }

    public PlayerData Load()
    {
        var result = new PlayerData();

        if (!File.Exists(savePath))
        {
            UnityEngine.Debug.Log("FILE NOT EXIST");
            return result;
        }
        string tempJson = File.ReadAllText(savePath);
        result = JsonUtility.FromJson<PlayerData>(tempJson);
        

        return result;
    }
}