using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    [Serializable]
    public class ShakeSettings
    {
        public PresetName PresetName;
        public float Coefficient;
    }

    [Serializable]
    public class PresetInfo
    {
        public PresetName PresetName;
        public NoiseSettings NoiseSettings;
        public float MaxAmplitude;
        public float MinAmplitude;
        public float Frequency;
    }

    [Serializable]
    public class FastShakeInfo
    {
        public float incrDur = 1.0f;
        public float decrDur = 1.0f;
        public float stable = 1.0f;
        public Ease incrEase = Ease.OutBack;
        public Ease decrEase = Ease.Linear;
        public float coeff;
    }

    public enum PresetName
    {
        Mild,
        Wobble,
        Strong,
        Extreme
    }

    public class ShakeController : MonoBehaviour
    {
        [SerializeField] private VCameraController _vCameraController;
        [Space, SerializeField, NonReorderable] private PresetInfo[] _presetInfos;
        [Space, SerializeField, NonReorderable] private ShakeSettings[] _shakeSettings;
        [Space, SerializeField, NonReorderable] private FastShakeInfo[] _fastShakeInfos;

        private const int OnFiredIndex = 0;
        private const int OnNuclearIndex = 1;
        
        private Coroutine _randomShakeCoroutine;
        private Dictionary<PresetName, (NoiseSettings, float, float, float)> _presets;
        private CinemachineBasicMultiChannelPerlin[] _perlins;

        private CinemachineVirtualCamera[] Cameras => _vCameraController.Cameras;

        private void Awake()
        {
            _presets = _presetInfos.ToDictionary(info => info.PresetName, info => 
                (info.NoiseSettings, info.MaxAmplitude, info.MinAmplitude, info.Frequency));

            _perlins = new CinemachineBasicMultiChannelPerlin[Cameras.Length];
            
            for (int i = 0; i < Cameras.Length; i++)
            {
                var perlin = Cameras[i].AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                var listener = Cameras[i].gameObject.AddComponent<CinemachineImpulseListener>();
                listener.m_Gain = 1.0f;
                Cameras[i].AddExtension(listener);
                _perlins[i] = perlin;
                
                ShakeSettings settings = _shakeSettings[i];
                (NoiseSettings profile, float maxAmpl, float minAmpl, float freq) = _presets[settings.PresetName];
                perlin.m_AmplitudeGain = Mathf.Max(maxAmpl * settings.Coefficient, minAmpl); 
                perlin.m_FrequencyGain = freq;
                perlin.m_NoiseProfile = profile;
            }
        }

        public void Shake(CinemachineBasicMultiChannelPerlin perlin, FastShakeInfo info)
        {
            float cached = perlin.m_AmplitudeGain;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(perlin.DoAmplitude(cached * info.coeff, info.incrDur).SetEase(info.incrEase));
            sequence.AppendInterval(info.stable);
            sequence.Append(perlin.DoAmplitude(cached, info.decrDur).SetEase(info.decrEase));
        }
        
        public void OnFired()
        {
            Shake(_perlins[_vCameraController.CurrentIndex], _fastShakeInfos[OnFiredIndex]);
        }

        public void OnNuclear()
        {
            Extensionss.Wait(0.4f).OnComplete(() =>
                Shake(_perlins[_vCameraController.CurrentIndex], _fastShakeInfos[OnNuclearIndex]));
        }
    }
}