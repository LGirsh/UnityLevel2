using System.IO;
using System;

public class StreamData : ISaveData
{
    string savePath = Path.Combine(UnityEngine.Application.dataPath, "StreamData.XYZ");

    public void Save(PlayerData player)
    {
        using(StreamWriter _writer = new StreamWriter(savePath))
        {
            _writer.WriteLine(player.PlayerName);
            _writer.WriteLine(player.PlayerHealth);
            _writer.WriteLine(player.PlayerDead);

        }
    }

    public PlayerData Load()
    {
        var result = new PlayerData();

        if (!File.Exists(savePath))
        {
            UnityEngine.Debug.Log("FILE NOT EXIST");
            return result;
        }
        using(StreamReader _reader = new StreamReader(savePath))
        {
            result.PlayerName = _reader.ReadLine();
            result.PlayerHealth = Convert.ToInt32(_reader.ReadLine());
            result.PlayerDead = Convert.ToBoolean(_reader.ReadLine());

        }

        return result;
    }
}