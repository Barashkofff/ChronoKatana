using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowMo : MonoBehaviour
{
    #region Singleton

    public static SlowMo instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public Slider TimeBar;
    public float slowMoTime;
    public float slowMoOffset;

    private float _timer;
    private bool _onCoolDown = true;

    void Start()
    {
        _timer = slowMoTime;
        TimeBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (_timer > slowMoOffset && _onCoolDown)
            {
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = Time.timeScale * .02f / 2; //После фикса камеры убрать /2

                _onCoolDown = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) || _timer < 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f / 2; //После фикса камеры убрать /2

            _onCoolDown = true;
        }

        if (_onCoolDown)
        {
            if (_timer < slowMoTime)
                _timer += Time.deltaTime;
        }
        else
            _timer -= Time.deltaTime * 2;

        TimeBar.value = (slowMoTime - _timer) / slowMoTime;
    }
}
