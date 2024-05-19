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
            SceneManager.LoadScene(data.sceneName, LoadSceneMode.Single);
        else
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            Debug.Log("√ƒ≈ “€ ¡Àﬂ“‹");
        }
    }

    void ISaveLoader.SaveData()
    {
        string level = SceneManager.GetActiveScene().name;
        Debug.Log("¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿ " + level);
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

        Debug.Log(PlayerController.instance.CurTable);
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
        if (SceneManager.GetActiveScene().name == "Learning")
            return;
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
        if (SceneManager.GetActiveScene().name == "Learning")
            return;
        List<GameObject> list = EnemyController.EnemiesList();
        EnemyData[] dataSet = new EnemyData[list.Count];
        Debug.Log(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(i);
            GameObject enemy = list[i];
            Debug.Log(i + " " + i);
            dataSet[i] = new EnemyData
            {
                _name = enemy.name,
                _enabled = enemy.activeSelf
            };
            Debug.Log(i);
        }
        EnemyController.Clear();
        Repository.SetData(dataSet);
    }
}

[Serializable]
public struct PlayerData
{
    public bool _double_jump;
    public bool _dash;
    public bool _slowmo;
    public bool _cool_dash;
}

public sealed class PlayerSaveLoader : ISaveLoader
{
    void ISaveLoader.LoadData()
    {
        if (SceneManager.GetActiveScene().name == "Learning")
            return;
        PlayerData data;
        if (!Repository.TryGetData<PlayerData>(out data))
            return;

        PlayerController.instance.SetAbles(data._double_jump, data._dash, data._cool_dash);
        if (data._slowmo)
            SlowMo.instance.slowMo_SetTrue();
    }

    void ISaveLoader.SaveData()
    {
        if (SceneManager.GetActiveScene().name == "Learning")
            return;

        PlayerData data = new PlayerData
        {
            _double_jump = PlayerController.instance.GetDoubleJump(),
            _dash = PlayerController.instance.GetDash(),
            _slowmo = SlowMo.instance.GetSlowMo(),
            _cool_dash = PlayerController.instance.GetCoolDash()
        };
        Repository.SetData(data);
    }
}

