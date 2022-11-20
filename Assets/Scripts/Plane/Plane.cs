
using DG.Tweening;
using Extensions;
using PathCreation;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Plane : TimelineObject
{
    [SerializeField] protected int _currentSceneIndex;
    [SerializeField] protected float _t;
    [SerializeField] private Transform _planeModel;

    [SerializeField, NonReorderable, Space]
    protected PathCreator[] _pathCreators;

    [Space] [SerializeField] private ParticleSystem _personalExplosion;
    [SerializeField] private float _delay;
    [SerializeField] private ParticleSystem _personalSmokeFlow;
    [Space, SerializeField] private AudioSource _explosionSound;
    [SerializeField] private Transform _blades;

    public void OnAwake()
    {
        Transform backWheel = _planeModel.GetChild(1);
        backWheel.gameObject.SetActive(false);

        Transform backWheelRightDoor = _planeModel.GetChild(5);
        backWheelRightDoor.localPosition = new Vector3(-0.00034f, -0.01463f, 0.00252f);
        backWheelRightDoor.localEulerAngles = new Vector3(-8.781f, -135.931f, -9.339f);
        
        Transform backWheelLeftDoor = _planeModel.GetChild(4);
        backWheelLeftDoor.localPosition = new Vector3(0.000387558f, -0.01465257f, 0.002444201f);
        backWheelLeftDoor.localEulerAngles = new Vector3(-8.696f, 135.409f, 8.216f);

        Transform rightLegOfWheel = _planeModel.GetChild(11);
        rightLegOfWheel.localPosition = new Vector3(-0.00461f, 0.00671f, 0.00575f);
        rightLegOfWheel.localEulerAngles = new Vector3(248.635f, 126.784f, 328.386f);

        Transform wheelRight = _planeModel.GetChild(11).GetChild(0);
        wheelRight.localPosition = new Vector3(-0.0002783525f, 0.001402789f, -0.004396218f);
        wheelRight.localEulerAngles = Vector3.zero;

        Transform leftLegOfWheel = _planeModel.GetChild(7);
        leftLegOfWheel.localPosition = new Vector3(0.00449f, 0.00664f, 0.00592f);
        leftLegOfWheel.localEulerAngles = new Vector3(245.593f, 226.945f, 43.62399f);

        Transform wheelLeft = _planeModel.GetChild(7).GetChild(0);
        wheelLeft.localPosition = new Vector3(0.0002783525f, 0.001402789f, -0.004396218f);
        wheelLeft.localEulerAngles = Vector3.zero;

        _blades = _planeModel.GetChild(6);

        Transform leftWheelLeftDoor = _planeModel.GetChild(8);
        leftWheelLeftDoor.localPosition = new Vector3(0.005211f, 0.00615f, 0.004947f);
        leftWheelLeftDoor.localEulerAngles = new Vector3(-6.819f, 66.22f, 9.368f);

        Transform leftWheelRightDoor = _planeModel.GetChild(9);
        leftWheelRightDoor.localPosition = new Vector3(0.00367f, 0.005769f, 0.004912f);
        leftWheelRightDoor.localEulerAngles = new Vector3(-3.789f, -45.613f, -8.751f);

        Transform rightWheelLeftDoor = _planeModel.GetChild(12);
        rightWheelLeftDoor.localPosition = new Vector3(-0.003626f, 0.005752f, 0.004914f);
        rightWheelLeftDoor.localEulerAngles = new Vector3(-6.297f, 50.603f, 7.022f);

        Transform rightWheelRightDoor = _planeModel.GetChild(13);
        rightWheelRightDoor.localPosition = new Vector3(-0.00524f, 0.00617f, 0.00496f);
        rightWheelRightDoor.localEulerAngles = new Vector3(-6.98f, -67.584f, -7.292f);
    }
    
    private void LateUpdate()
    {
        UpdateTransform();
        _blades.localEulerAngles += new Vector3(0, 1, 0) * -1000f * Time.deltaTime;
    }

    protected virtual void UpdateTransform()
    {
        transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
        transform.rotation = _pathCreators[_currentSceneIndex].path.GetRotation(_t);
    }
    
#if UNITY_EDITOR
    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        //Debug.Log("VAR");
        UpdateTransform();
    }
#endif
   

    public void OnFarSceneStarted()
    {
        Debug.LogError("on far started");
        _planeModel.localRotation = Quaternion.Euler(-60, 0, 180);
    }

    public void OnFarSceneEnded()
    {
        Debug.LogError("on far ended");
        _planeModel.localRotation = Quaternion.Euler(-90, 0, 180);
    }

    public void SetPlaneRot(Quaternion rot)
    {
        _planeModel.localRotation = rot;
    }

    public void Explode()
    {
        Debug.LogError("explode1");
        if (_explosionSound != null)
        {
            _explosionSound.Play();
        }
        _personalExplosion.Play(true);
        _personalExplosion.transform.parent = null;
        Extensionss.Wait(_delay).OnComplete(() => _personalSmokeFlow.Play(true));
    }
}