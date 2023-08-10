using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuestBear))]
public class GuestBearEditor : Editor
{
    private void OnSceneGUI()
    {
        GuestBear fov = (GuestBear)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        Vector3 viewAngle01 = DirFromAngle(fov.transform.eulerAngles.y, -fov.viewAngle / 2);
        Vector3 viewAngle02 = DirFromAngle(fov.transform.eulerAngles.y, fov.viewAngle / 2);
        
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.viewRadius);
        
        if (fov.pairPlayer != null)
        {
            Handles.color = Color.magenta;
            Handles.DrawLine(fov.transform.position, fov.pairPlayer.transform.position);
        }
    }

    private Vector3 DirFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
