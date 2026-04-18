using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private const string KEY = "CardMatch_Save";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public static SaveData Load()
    {
        if (!PlayerPrefs.HasKey(KEY))
            return null;

        string json = PlayerPrefs.GetString(KEY);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(KEY);
    }
}

[System.Serializable]
public class SaveData
{
    public List<int> cardIndices;  // index in cardPool
    public List<bool> matched;
}
