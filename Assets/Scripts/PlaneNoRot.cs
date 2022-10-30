public class PlaneNoRot : Plane
{
    protected override void UpdateTransform()
    {
        if(gameObject.activeSelf)
            transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
    }
}