using UnityEditor;
using FateGames;
[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraFollow cameraFollow = (CameraFollow)target;
        if (DrawDefaultInspector())
        {
            cameraFollow.TakePosition();
        }
    }
}
