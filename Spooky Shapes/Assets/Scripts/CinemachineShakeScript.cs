using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShakeScript : MonoBehaviour
{
    public static CinemachineShakeScript Instance { get; private set;}

    private CinemachineVirtualCamera CVM;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startIntensity;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        CVM = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = CVM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera(float intensity, float t) {
        startIntensity = intensity;
        shakeTimer =  t;
        shakeTimerTotal = t;
        StopAllCoroutines();
        StartCoroutine(shake());

    }

    private IEnumerator shake() {

        while (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
                perlinNoise.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, shakeTimer / shakeTimerTotal);
                yield return new WaitForEndOfFrame();
        }
        perlinNoise.m_AmplitudeGain = 0;

    }

}
