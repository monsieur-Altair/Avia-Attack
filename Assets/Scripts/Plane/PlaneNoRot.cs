public class PlaneNoRot : Plane
{
    protected override void UpdateTransform()
    {
        if(gameObject.activeSelf && _pathCreators[_currentSceneIndex] != null)
            transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
    }
}