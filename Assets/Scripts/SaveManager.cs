using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveManager
{
    public SaveData saveData { get; private set; }

    private string filePath;

    SaveManager()
    {
        saveData = new SaveData();
        filePath = Path.Combine(Application.persistentDataPath, "data.json");

        Load();
    }

    private void Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"SaveManager: Load: json={json}");

            saveData = JsonUtility.FromJson<SaveData>(json);
        }

        Debug.Log($"SaveManager: Load: saveData.level={saveData.level}");
    }

    public void SaveLevel(int level)
    {
        saveData.level = level;
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(filePath, json);

        Debug.Log($"SaveManager: SaveLevel: JSON saved to: {filePath}");
    }

    public int GetLevel()
    {
        return saveData.level;
    }
}
