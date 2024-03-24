using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitCam : MonoBehaviour
{
    public AnimationCurve curve;
    public Camera mainCam;
    public GameObject tabletFullRes;

    private Camera transitCam;

    private Vector3 targetPos;
    private Vector3 startPos;

    private float startSize;
    private float targetSize;
    

    void Start()
    {
        PlayerController.instance.enabled = false;
        mainCam.GetComponent<CameraController>().FindPlayer(true);

        transitCam = GetComponent<Camera>();
        startSize = transitCam.orthographicSize;
        targetSize = mainCam.orthographicSize;

        startPos = transform.position;
        targetPos = mainCam.transform.position;
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transitCam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            t += 0.005f;
            yield return new WaitForEndOfFrame();
        }

        GetComponent<Camera>().enabled = false;
        PlayerController.instance.enabled = true;
    }
}
