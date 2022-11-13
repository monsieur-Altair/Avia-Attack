using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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

    public enum PresetName
    {
        Mild,
        Wobble,
        Strong,
        Extreme
    }

    public class ShakeController : MonoBehaviour
    {
        [SerializeField] private float _strength = 12f;
        [SerializeField] private VCameraController _vCameraController;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [Space, SerializeField, NonReorderable] private PresetInfo[] _presetInfos;
        [Space, SerializeField, NonReorderable] private ShakeSettings[] _shakeSettings;

        private Coroutine _randomShakeCoroutine;
        private Dictionary<PresetName, (NoiseSettings, float, float, float)> _presets;

        private CinemachineVirtualCamera[] Cameras => _vCameraController.Cameras;

        private void Awake()
        {
            _presets = _presetInfos.ToDictionary(info => info.PresetName, info => 
                (info.NoiseSettings, info.MaxAmplitude, info.MinAmplitude, info.Frequency));
            
            for (int i = 0; i < Cameras.Length; i++)
            {
                var perlin = Cameras[i].AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                Cameras[i].AddExtension(Cameras[i].gameObject.AddComponent<CinemachineImpulseListener>());
                
                ShakeSettings settings = _shakeSettings[i];
                (NoiseSettings profile, float maxAmpl, float minAmpl, float freq) = _presets[settings.PresetName];
                perlin.m_AmplitudeGain = Mathf.Max(maxAmpl * settings.Coefficient, minAmpl); 
                perlin.m_FrequencyGain = freq;
                perlin.m_NoiseProfile = profile;
            }
        }

        public void OnFired()
        {
            _impulseSource.GenerateImpulse(UnityEngine.Random.insideUnitSphere * _strength);
        } 
    }
}