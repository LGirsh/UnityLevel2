using System.IO;
using System;
using System.Xml;

public class XMLData : ISaveData
{

    string savePath = Path.Combine(UnityEngine.Application.dataPath, "XMLData.xml");

    public void Save(PlayerData player)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("Player");
        xmlDoc.AppendChild(rootNode);

        XmlElement element = xmlDoc.CreateElement("Name");
        element.SetAttribute("value", player.PlayerName);
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Health");
        element.SetAttribute("value", player.PlayerHealth.ToString());
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Dead");
        element.SetAttribute("value", player.PlayerDead.ToString());
        rootNode.AppendChild(element);

        xmlDoc.Save(savePath);
    }



    public PlayerData Load()
    {
        var result = new PlayerData();

        if (!File.Exists(savePath))
        {
            UnityEngine.Debug.Log("FILE NOT EXIST");
            return result;
        }

        using (XmlTextReader reader = new XmlTextReader(savePath))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement("Name"))
                {
                    result.PlayerName = reader.GetAttribute("value");
                }
                if (reader.IsStartElement("Health"))
                {
                    Int32.TryParse(reader.GetAttribute("value"), out result.PlayerHealth);
                }
                if (reader.IsStartElement("Dead"))
                {
                    result.PlayerDead = Convert.ToBoolean(reader.GetAttribute("value"));
                }
            }
        }
        return result;
    }
}
