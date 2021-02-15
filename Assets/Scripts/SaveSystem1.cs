using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public static class SaveSystem1
{
    public static void SaveHighScore1(int hs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Flappy_Bird.kt";
        //Debug.Log("Path Created : " + path);
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate); 

        SaveData1 save = new SaveData1();

        if (File.Exists(path) && stream.Length > 0)
        {
            stream.Close();
            //Debug.Log("File Exists");
            FileStream stream2 = new FileStream(path, FileMode.Open);
            SaveData1 data = (SaveData1)formatter.Deserialize(stream2);
            //Debug.Log(data.scores[0]);
            stream2.Close();

            int changeIndex = -1;
            {            //find index at which to change score
                for (int i = 0; i < 4; i++)
                {
                    if (hs > data.scores[i])
                    {
                        changeIndex = i;
                        //Debug.Log(changeIndex);
                        break;
                    }
                } 
            }

            //check if update is necessary
            {
                if (changeIndex > -1)
                {
                    int[] tempArray = new int[4];

                    //1st half of array
                    for (int tempIndex = 0; tempIndex < changeIndex; tempIndex++)      //consider case for 0 also
                    {
                        tempArray[tempIndex] = data.scores[tempIndex];
                    }

                    if(changeIndex == 0)                 //as above loop will terminate if changeIndex is 0
                    {
                        tempArray[changeIndex] = hs;
                    }
                    tempArray[changeIndex] = hs;         //insert HIghScore

                    //2nd half of array
                    for (int tempIndex = changeIndex + 1; tempIndex < 4; tempIndex++)
                    {
                        tempArray[tempIndex] = data.scores[tempIndex - 1];

                        {            //old code
                            /*
                            if (tempIndex > 0)
                            {
                                tempArray[tempIndex] = data.scores[tempIndex - 1];
                            }
                            if (tempIndex == changeIndex)
                            {
                                tempArray[tempIndex] = hs;
                                continue;
                            }
                            else
                            {
                            }
                            */
                        }
                    }

                    data.scores = tempArray;
                    FileStream stream3 = new FileStream(path, FileMode.Create);
                    formatter.Serialize(stream3, data);
                    //Debug.Log("File Created :  " + data.scores[0]);
                    stream3.Close();
                }
            }
        }
        else
        {
            save.scores[0] = hs;                       //save current score a high score, in the 1st slot
            formatter.Serialize(stream, save);
            stream.Close();
        }
    }

    public static int[] LoadScore1()
    {
        string path = Application.persistentDataPath + "/Flappy_Bird.kt";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData1 data = (SaveData1)formatter.Deserialize(stream);
            int[] tempScoreArray = { };
            stream.Close();
            return tempScoreArray;
        }
        else
        {
            return null;
        }
    }

    public static int LoadHighScore1(int[] tempScoreArray, bool mScore)
    {
        string path = Application.persistentDataPath + "/Flappy_Bird.kt"; 
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData1 data = (SaveData1)formatter.Deserialize(stream);
            if ((tempScoreArray != null) && mScore)
            {
                for (int i = 0; i < 4; i++)
                {
                    tempScoreArray[i] = data.scores[i];
                }
            }

            int loadScore = data.scores[0];                            // the topmost value will be the high score, if it is sorted

            stream.Close();

            return loadScore;
        }

        else
        {
            //Debug.Log("File Not Found");
            return 0;
        }
    }
}

[System.Serializable]
public class SaveData1
{
    public int[] scores = new int[4] { 0, 0, 0, 0 }; 
}
