using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogData
{
    public string name;
    public string dialog;
    public bool bIsChoice;
    public int redirectIndex;
    public Emotion emotion;
    public CharacterType type;
    public bool bToggleTimer;
    public float addedTimerValue;
}

public static class CSVReader
{
    // offset is -2
    private static int lineOffset = -2;

    public static List<DialogData> MakeDialogData(TextAsset csv)
    {
        List<DialogData> dialogsData = new List<DialogData>();
        string[] lines = csv.text.Split("\n");
        string lastName = "";

        int index = 0;
        int collumnCount = lines[0].Split(',').Length;

        // the first line is text info only
        for(int i = 1; i < lines.Length; i++)
        {
            DialogData data = new DialogData();
            string[] collumns = new string[collumnCount];

#if UNITY_EDITOR
            string[] a = lines[i].Split('<');
            string[] b = lines[i].Split('>');

            if (a.Length != b.Length)
            {
                Debug.LogWarning("Unclosed tag at " + ((i - lineOffset) - 1));
            }
#endif
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
            // collumn 4: addedTimerValue
            // collumn 5: bToggleTimer
            // collumn 6: emotion
            data.name = collumns[0] == "" ? lastName : collumns[0];
            lastName = data.name;

            if (Enum.TryParse<CharacterType>(data.name, out CharacterType type))
            {
                data.type = type;
            }
            else
            {
                data.type = CharacterType.Client;
            }

            data.dialog = collumns[1];
            data.bIsChoice = collumns[2] != "";

#if UNITY_EDITOR
            if(data.bIsChoice && collumns[3] == "")
            {
                Debug.LogWarning((index - lineOffset) + " is a choice but does not have a redirectIndex.");
            }
#endif

            index++;

#if UNITY_EDITOR
            if(collumns[3] == null)
            {
                Debug.LogWarning("\\n in " + ((i - lineOffset) - 1));
            }
#endif  
            
            data.redirectIndex = collumns[3] != "" ? (int.Parse(collumns[3]) + lineOffset) : index;

            float.TryParse(collumns[4], out float floatRes);
            data.addedTimerValue = floatRes;

            data.bToggleTimer = collumns[5] != "";

            bool bFindEmotion = Enum.TryParse<Emotion>(collumns[6], out Emotion emotion);
            data.emotion = bFindEmotion ? emotion : Emotion.None;

            dialogsData.Add(data);
        }

        return dialogsData;
    }
}
