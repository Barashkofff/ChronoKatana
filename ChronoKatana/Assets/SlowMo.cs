using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    #region Singleton

    public static SlowMo instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public float slowMoTime;
    public float slowMoOffset;

    private float _timer;
    private bool _onCoolDown = true;

    void Start()
    {
        _timer = slowMoTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_timer > slowMoOffset)
                Time.timeScale = 0.5f;
            _onCoolDown = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (_timer > slowMoOffset && _onCoolDown)
            {
                Time.timeScale = 0.5f;
                _onCoolDown = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Time.timeScale = 1f;
            _onCoolDown = true;
        }
        
        if (_timer < 0)
        {
            Time.timeScale = 1f;
            _onCoolDown = true;
        }

        if (_onCoolDown)
        {
            if (_timer < slowMoTime)
                _timer += Time.deltaTime;
        }
        else
            _timer -= Time.deltaTime * 2;
    }
}
