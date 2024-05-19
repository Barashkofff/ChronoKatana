using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroObjects : MonoBehaviour
{

    [SerializeField] private TransitCam transCam;

    [HideInInspector]
    public static IntroObjects instance;

    public Transform[] tables;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Repository.LoadState();
        if (SceneManager.GetActiveScene().name == "Learning")
        {
            transform.position = tables[0].position;
            PlayerController.instance.transform.position = tables[0].position - new Vector3(2, 0, 0);
            tables[0].GetComponent<TabletScript>().UpdateSprite();
            transCam.Transition();
            return;
        }
        SaveLoadManager.LoadGame();
        int tableInd = PlayerController.instance.CurTable;
        Debug.Log("INDEX_LOADED");
        transform.position = tables[tableInd].position;
        PlayerController.instance.transform.position = tables[tableInd].position - new Vector3(2, 0, 0);
        UpdateTables();
        transCam.Transition();

        EnemySaveLoader esl = new EnemySaveLoader();
        (esl as ISaveLoader).LoadData();

        if (!Repository.TryGetData<TableData>(out TableData data) && SceneManager.GetActiveScene().name != "Level1") 
        { 
            End.instance.fadePanel.enabled = true;

            var datax = new TableData
            {
                index = 0
            };
            Repository.SetData(data);
        }
        Debug.Log("22");
        LevelSaveLoader levelSaveLoader = new LevelSaveLoader();
        (levelSaveLoader as ISaveLoader).SaveData();

        Repository.SaveState();
        Debug.Log("2222");
    }

    public void UpdateTables()
    {
        foreach (var table in tables)
        {
            table.GetComponent<TabletScript>().UpdateSprite();
        }
    }

    [ContextMenu("—брос чекпоинтов")]
    public void ResetSpawn()
    {
        Repository.DeleteData<TableData>();
        Repository.SaveState();
    }

    [ContextMenu("—брос уровн€")]
    public void ResetLevel()
    {
        Repository.DeleteData<LevelData>();
        Repository.SaveState();
    }

    [ContextMenu("—брос врагов")]
    public void ResetEnemies()
    {
        Repository.DeleteData<EnemyData[]>();
        Repository.SaveState();
    }

    [ContextMenu("—брос способностей")]
    public void ResetAbilities()
    {
        Repository.DeleteData<PlayerData>();
        Repository.SaveState();
    }

    [ContextMenu("—брос всего")]
    public void ResetAll()
    {
        ResetSpawn();
        ResetEnemies();
        ResetAbilities();
        ResetLevel();
    }
}
