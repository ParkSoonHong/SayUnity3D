using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;

        transform.position = newPosition;

        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;

    }
}
