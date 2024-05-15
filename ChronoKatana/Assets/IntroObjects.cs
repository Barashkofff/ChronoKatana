using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroObjects : MonoBehaviour
{
    [SerializeField] private TransitCam transCam;

    [HideInInspector]
    public static IntroObjects instance;

    public Transform[] tables;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SaveLoadManager.LoadGame();
        int tableInd = PlayerController.instance.CurTable;
        Debug.Log("INDEX_LOADED");
        transform.position = tables[tableInd].position;   
        PlayerController.instance.transform.position = tables[tableInd].position - new Vector3(2, 0, 0);

        transCam.Transition();
    }

    [ContextMenu("—брос чекпоинтов")]
    public void ResetSpawn()
    {
        var data = new TableData
        {
            index = 0
        };
        Repository.SetData(data);
        Repository.SaveState();
    }
}
