using System;
using DG.Tweening;
using Extensions;
using PathCreation;
using UnityEditor;
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

    private void LateUpdate()
    {
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
        transform.rotation = _pathCreators[_currentSceneIndex].path.GetRotation(_t);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);

        UpdateTransform();
    }

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
        _personalExplosion.Play(true);
        _personalExplosion.transform.parent = null;
        Extensionss.Wait(_delay).OnComplete(() => _personalSmokeFlow.Play(true));
    }
}