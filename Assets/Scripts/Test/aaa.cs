using System;
using Cinemachine;
using DefaultNamespace;
using DG.Tweening;
using Extensions;
using UnityEngine;

public class aaa : MonoBehaviour
{
    public CinemachineImpulseSource Source;
    public CinemachineVirtualCamera Camera;
    public float Base;
    public float Coeff;
    private CinemachineBasicMultiChannelPerlin _perlin;
    

    private void Awake()
    {
        _perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Shake(_perlin, null);
        }
    }


    public void Shake(CinemachineBasicMultiChannelPerlin perlin, FastShakeInfo info)
    {
        float cached = perlin.m_AmplitudeGain;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(perlin.DoAmplitude(cached * Coeff, info.incrDur).SetEase(info.incrEase));
        sequence.AppendInterval(info.stable);
        sequence.Append(perlin.DoAmplitude(cached, info.decrDur).SetEase(info.decrEase));
    }
}