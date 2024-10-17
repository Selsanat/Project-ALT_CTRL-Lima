using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueData
{
    public string name;
    public string dialogue;
    public bool bIsChoice;
    public int redirectIndex;
    public Emotion emotion;
}

public static class CSVReader
{
    // offset is -2
    private static int lineOffset = -2;

    public static List<DialogueData> MakeDialogueData(TextAsset csv)
    {
        List<DialogueData> dialoguesData = new List<DialogueData>();
        string[] lines = csv.text.Split("\n");
        string lastName = "";

        int index = 0;
        int collumnCount = lines[0].Split(',').Length;

        // the first line is text info only
        for(int i = 1; i < lines.Length; i++)
        {
            DialogueData data = new DialogueData();
            string[] collumns = new string[collumnCount];

            if (lines[i].Contains('"'))
            {
                string[] result = lines[i].Split('"');

                int targetIndex = -1;
                string remainingStr = string.Empty;
                for(int j = 0; j < result.Length; j++)
                {
                    string currentStr = result[j];
                    if (currentStr[0] == ',' || currentStr[currentStr.Length - 1] == ',')
                    {
                        remainingStr += currentStr;
                        continue;
                    }

                    targetIndex = j;
                }

                collumns[targetIndex] = result[targetIndex];

                string[] splitedStr = remainingStr.Split(",");
                for (int j = 0; j < splitedStr.Length; j++)
                {
                    if(j == targetIndex)
                    {
                        continue;
                    }

                    collumns[j] = splitedStr[j];
                }
            }
            else
            {
                collumns = lines[i].Split(",");
            }

            // collumn 0: characterName
            // collumn 1: text
            // collumn 2: choice
            // collumn 3: redirectTo
            // collumn 4: emotion
            data.name = collumns[0] == "" ? lastName : collumns[0];
            lastName = data.name;

            data.dialogue = collumns[1];
            data.bIsChoice = collumns[2] != "";

            index++;
#if UNITY_EDITOR
            if(data.bIsChoice && collumns[3] == "")
            {
                Debug.LogWarning((index + 1) + " is a choice but does not have a redirectIndex.");
            }
#endif
            data.redirectIndex = collumns[3] != "" ? (int.Parse(collumns[3]) + lineOffset) : index;

            bool bFindEmotion = Enum.TryParse<Emotion>(collumns[4], out Emotion emotion);
            data.emotion = bFindEmotion ? emotion : Emotion.None;

            dialoguesData.Add(data);
        }

        return dialoguesData;
    }
}
