using UnityEngine;
using UnityEngine.Playables;

namespace DefaultNamespace
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _timeline;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            
            if(Input.GetKeyDown(KeyCode.P))
            {
                _timeline.Pause();    
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _timeline.Resume();
            }
        }
    }
}