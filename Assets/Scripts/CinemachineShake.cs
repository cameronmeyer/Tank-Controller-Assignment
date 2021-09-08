using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _cvc;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    public void Shake(float intensity, float timer)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = _cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = intensity;

        _startingIntensity = intensity;
        _shakeTimer = timer;
        _shakeTimerTotal = timer;
    }

    private void Update()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if(_shakeTimer <= 0f)
            {
                // Timer Finished
                CinemachineBasicMultiChannelPerlin cbmcp = _cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cbmcp.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
        }
    }
}
