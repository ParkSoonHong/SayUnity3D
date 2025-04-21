using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150;

    private float _rotationX = 0;

    void Update()
    {
        // 1. ���콺 �Է��� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");

        // 2. ȸ���� �縸ŭ �������� ������.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
