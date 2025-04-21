using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150;

    private float _rotationX = 0;

    void Update()
    {
        // 1. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
