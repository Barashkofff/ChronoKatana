using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveLoadManager
{
    

    private static readonly ISaveLoader[] saveLoaders = {
        //new LevelSaveLoader(),
        new TableIndexSaveLoader()
    };

    public static void LoadGame()
    {
        Repository.LoadState();

        foreach (var saveLoader in saveLoaders)
        {
            saveLoader.LoadData();
        }
    }

    public static void SaveGame()
    {
        foreach (var saveLoader in saveLoaders)
        {
            saveLoader.SaveData();
        }

        Repository.SaveState();
    }
}

public interface ISaveLoader
{
    void LoadData();
    void SaveData();
}


[Serializable]
public struct LevelData
{
    public string sceneName;
}

public sealed class LevelSaveLoader : ISaveLoader
{
    void ISaveLoader.LoadData()
    {
        LevelData data;
        if (Repository.TryGetData<LevelData>(out data))
            SceneManager.LoadScene(data.sceneName);
        else
            SceneManager.LoadScene("Level1");
    }

    void ISaveLoader.SaveData()
    {
        string level = SceneManager.GetActiveScene().name;
        var data = new LevelData
        {
            sceneName = level
        };
        Repository.SetData(data);
    }
}

[Serializable]
public struct TableData
{
    public int index;
}

public sealed class TableIndexSaveLoader : ISaveLoader
{
    void ISaveLoader.LoadData()
    {
        TableData data;
        if (Repository.TryGetData<TableData>(out data))
            PlayerController.instance.CurTable = data.index;
        else
            PlayerController.instance.CurTable = 0;
    }

    void ISaveLoader.SaveData()
    {
        int indexTable = PlayerController.instance.CurTable;
        var data = new TableData
        {
            index = indexTable
        };
        Repository.SetData(data);
    }
}