using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // ���� ��� interpoling, smooting ����� �� ����
        transform.position = Target.position;
    }
}
