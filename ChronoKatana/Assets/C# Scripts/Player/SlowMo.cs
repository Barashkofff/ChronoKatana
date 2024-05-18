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

    [SerializeField] private bool _ableSlowMo;

    public void slowMo_SetTrue() { _ableSlowMo = true; TimeBar.gameObject.SetActive(true); PlayerController.instance.SaveState(); }
    public bool GetSlowMo() { return _ableSlowMo; }

    void Start()
    {
        _timer = slowMoTime;
        TimeBar.value = 0;
        if (!_ableSlowMo)
            TimeBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ableSlowMo)
            return;
            

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.L))
        {
            if (_timer > slowMoOffset && _onCoolDown)
            {
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = Time.timeScale * .02f / 2; //После фикса камеры убрать /2

                _onCoolDown = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.L) || _timer < 0)
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
