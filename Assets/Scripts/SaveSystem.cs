using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static EndScreenController con = GameObject.FindObjectOfType<EndScreenController>();
    static PlayerData data;
    static string path = Application.persistentDataPath + "/player.alex";

    public static void SavePlayer(EndScreenController controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        data = new PlayerData();
        data.Score1 = controller.Scores[0];
        data.Score2 = controller.Scores[1];
        data.Score3 = controller.Scores[2];
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData GetData(int score1, int score2, int score3)
    {
        if (File.Exists(path))
        {
            // load in already-existing data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            return data;
        }
        
        else
        {
            // no player data found, create new save
            data = new PlayerData();
            data.Score1 = score1;
            data.Score2 = score2;
            data.Score3 = score3;
            return data;
        }
    }

    public static bool bSaveDataExists()
    {
        return File.Exists(path);
    }
}
