public class PlaneNoRot : Plane
{
    protected override void UpdateTransform()
    {
        transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
    }
}