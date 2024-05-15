using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

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

        if (SceneManager.GetActiveScene().name == "Learning")
        {
            PlayerController.instance.CurTable = 0;
            return;
        }

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

[Serializable]
public struct EnemyData
{
    public string _name;
    public bool _enabled;
}

public sealed class EnemySaveLoader : ISaveLoader
{
    void ISaveLoader.LoadData()
    {
        EnemyData[] dataSet;
        Debug.Log("LoadEnemies1");
        if (!Repository.TryGetData<EnemyData[]>(out dataSet))
            return;

        Debug.Log("LoadEnemies2");
        List<GameObject> list = EnemyController.EnemiesList();
        for (int i = 0; i < dataSet.Length; i++)
        {
            EnemyData data = dataSet[i];
            GameObject enemy = list.Find(x => x.name == data._name);
            enemy.SetActive(data._enabled);
        }
    }

    void ISaveLoader.SaveData()
    {
        List<GameObject> list = EnemyController.EnemiesList();
        EnemyData[] dataSet = new EnemyData[list.Count];
        Debug.Log("SaveEnemies");
        for (int i = 0; i < list.Count; i++)
        {
            GameObject enemy = list[i];
            dataSet[i] = new EnemyData
            {
                _name = enemy.name,
                _enabled = enemy.activeSelf
            };
            if (!enemy.activeSelf)
                Debug.Log(enemy.name + "!!!");
        }
        EnemyController.Clear();
        Repository.SetData(dataSet);
    }
}