using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveDataManager
{
    /// <summary>
    /// Saves the highscores in a permanent place.
    /// </summary>
    /// <param name="highScores"></param>
    public static void SaveData(LinkedList<HighScore> highScores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/HighScores.sav";
        FileStream stream = new FileStream(path, FileMode.Create);
        string data = ConvertDataOnSave(highScores);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Save done!");
    }

    /// <summary>
    /// Load the highscores from the permanent place.
    /// </summary>
    /// <returns></returns>
    public static LinkedList<HighScore> LoadData()
    {
        string path = Application.persistentDataPath + "/HighScores.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            LinkedList<HighScore> finalData = new LinkedList<HighScore>();

            string inputData = formatter.Deserialize(stream) as string;
            stream.Close();
            if(inputData.Length != 0)
                finalData = ConvertDataOnLoad(inputData);
            Debug.Log("Load done!");
            return finalData;
        }
        else //not found
        {
            Debug.Log("No file detected"); //this case need to be controled on the function who demands load
            return new LinkedList<HighScore>();
        }
    }

    /// <summary>
    /// Convert the highscores into a recuperable string.
    /// </summary>
    /// <param name="highScores"></param>
    /// <returns></returns>
    private static string ConvertDataOnSave(LinkedList<HighScore> highScores)
    {
        StringBuilder data = new StringBuilder();
        string finalData;
        LinkedListNode<HighScore> searchNode = highScores.First;

        data.Append("*start\n");

        while(searchNode != null)
        {
            data.Append("*next\n");
            data.Append(searchNode.Value.GetName() + "\n");
            data.Append(searchNode.Value.GetScore() + "\n");

            searchNode = searchNode.Next;
        }
        data.Append("*end\n");

        finalData = data.ToString();
        return finalData;
    }

    /// <summary>
    /// Convert the string saved into highscores.
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    private static LinkedList<HighScore> ConvertDataOnLoad(string inputData)
    {
        LinkedList<HighScore> finalData = new LinkedList<HighScore>();

        HighScore data;
        string temporalName;
        int temporalScore;

        bool end = false;
        int startRead = 0;
        int endRead = 0;
        char[] textCharacters = inputData.ToCharArray();
        string line;

        //this must contain the string "*start\n", otherelse it is an error.
        line = FindWord(ref textCharacters, ref startRead, ref endRead); 

        if (line != "*start")
        {
            Debug.LogError("Unknown character start");
        }
        else
        {
            line = FindWord(ref textCharacters, ref startRead, ref endRead);

            if(line == "*next") //there is data saved
            {
                while (!end)
                {
                    temporalName = FindWord(ref textCharacters, ref startRead, ref endRead);
                    temporalScore = System.Convert.ToInt32(FindWord(ref textCharacters, ref startRead, ref endRead));
                    data = new HighScore(temporalName, temporalScore);
                    finalData.AddLast(data);

                    line = FindWord(ref textCharacters, ref startRead, ref endRead);

                    if (line == "*end")
                    {
                        end = true;
                        Debug.Log("Load ok!");
                    }
                    else if(line != "*next")
                    {
                        end = true;
                        Debug.LogError("Unknown character ending");
                    }
                }
            }    
        }

        return finalData;
    }

    /// <summary>
    /// Reads one single word in the string.
    /// </summary>
    /// <param name="textCharacters"></param>
    /// <param name="startRead"></param>
    /// <param name="endRead"></param>
    /// <returns></returns>
    private static string FindWord(ref char[] textCharacters, ref int startRead, ref int endRead)
    {
        bool endFindWord = false;
        StringBuilder text = new StringBuilder();
        string finalText;
        //read and move to \n or to the end of file
        while (!endFindWord)
        {
            if (textCharacters[endRead] != "\n".ToCharArray()[0])
                endRead++;
            else
                endFindWord = true;
        }

        for (int i = startRead; i < endRead; i++)
            text.Append(textCharacters[i]);

        startRead = endRead + 1; //we ignore the \n and go to next valid character
        endRead = startRead;

        finalText = text.ToString();

        return finalText;
    }
}
