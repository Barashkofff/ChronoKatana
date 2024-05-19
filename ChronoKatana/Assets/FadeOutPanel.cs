using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableIndexSaveLoader tableIndexSaveLoader = new TableIndexSaveLoader();
        (tableIndexSaveLoader as ISaveLoader).LoadData();
        //PlayerController.
    }
}
