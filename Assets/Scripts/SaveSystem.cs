using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]

public static class SaveSystem
{ 
    public static void SaveHighScore(int hs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Flappy_Bird.kt";
        //Debug.Log("Path Created : " + path);
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);  

        if (File.Exists(path) && stream.Length > 0)
        {
            stream.Close();
            Debug.Log("File Exists");
            FileStream stream2 = new FileStream(path, FileMode.Open);
            int? tempHighScore = (int?)formatter.Deserialize(stream2); 
            stream2.Close();
            if(tempHighScore < hs)
            {
                FileStream stream3 = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream3, hs);
                stream3.Close();
            }
        }
        else
        { 
            formatter.Serialize(stream, hs);
            stream.Close();
        } 
    }

    public static int? LoadHighScore()
    {
        string path = Application.persistentDataPath + "/Flappy_Bird.kt";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open); 
            int? loadScore = (int?)formatter.Deserialize(stream);

            stream.Close(); 

            return loadScore;
        }

        else
        {
            Debug.Log("File Not Found");
            return 0;
        }
    }
}
