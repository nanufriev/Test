using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadSystem
{
    private SaveData _saveData;
    private readonly string _jsonSavePath = Application.persistentDataPath + "/saveload.json";

    public List<Item> LoadData()
    {
        if (File.Exists(_jsonSavePath))
            try
            {
                var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(_jsonSavePath)); ;

                return data.SavedItems;
            }
            catch
            {
                return new List<Item>();
            }
        else
            return new List<Item>();
    }

    public void SaveData(List<Item> items)
    {
        if (_saveData == null)
            _saveData = new SaveData();

        _saveData.SavedItems = items;

        string jsonData = JsonUtility.ToJson(_saveData);

        if (!File.Exists(_jsonSavePath))
            File.Create(_jsonSavePath);

        File.WriteAllText(_jsonSavePath, jsonData);

    }
}

[Serializable]
public class SaveData
{
    [SerializeField]
    public List<Item> SavedItems = new List<Item>();
}
