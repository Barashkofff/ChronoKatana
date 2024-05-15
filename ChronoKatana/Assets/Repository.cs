using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Repository
{
    private const string GAME_STATE_KEY = "GameState";

    private static Dictionary<string, string> currentState = new();



    //Загрузить данные с диска
    public static void LoadState()
    {
        if (PlayerPrefs.HasKey(GAME_STATE_KEY))
        {
            var serializedState = PlayerPrefs.GetString(GAME_STATE_KEY);
            currentState = JsonConvert.
              DeserializeObject<Dictionary<string, string>>(serializedState);
        }
        else
        {
            currentState = new Dictionary<string, string>();
        }
    }

    //Сохранить данные на диск
    public static void SaveState()
    {
        var serializedState = JsonConvert.SerializeObject(currentState, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        PlayerPrefs.SetString(GAME_STATE_KEY, serializedState);
    }

    public static T GetData<T>()
    {
        var serializedData = currentState[typeof(T).Name];
        return JsonConvert.DeserializeObject<T>(serializedData);
    }

    public static void SetData<T>(T value)
    {
        var serializedData = JsonConvert.SerializeObject(value);
        currentState[typeof(T).Name] = serializedData;
    }

    public static bool TryGetData<T>(out T value)
    {
        if (currentState.TryGetValue(typeof(T).Name, out var serializedData))
        {
            value = JsonConvert.DeserializeObject<T>(serializedData);
            return true;
        }

        value = default;
        return false;
    }

    public static void DeleteData<T>()
    {
        if (currentState.TryGetValue(typeof(T).Name, out var serializedData))
        {
            currentState.Remove(typeof(T).Name);
        }
    }
}


