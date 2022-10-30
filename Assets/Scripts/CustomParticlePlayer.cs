using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomParticlePlayer : MonoBehaviour
    {
        [Space] 
        [SerializeField] private ParticleSystem _personalExplosion;
        [SerializeField] private ParticleSystem _personalSmokeFlow;
        public void Play1()
        {
            if (_personalExplosion != null)
            {
                _personalExplosion.transform.parent = null;
                _personalExplosion.Play(true);
            }
            if(_personalSmokeFlow!=null)
                _personalSmokeFlow.Play(true);
        }
        
    }
}