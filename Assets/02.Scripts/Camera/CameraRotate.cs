using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 있다.
    // 구현 순서

    public float RotationSpeed = 150;

    // 카메라 각도는 0도에서 부터 시작한다고 기준을 세운다.
    private float _rotationX = 0;
    private float _rotationY = 0;

    private CameraFollow _cameraFollow;

    private void Awake()
    {
        _cameraFollow = GetComponent<CameraFollow>();
    }

    private void Update()
    {
        switch(_cameraFollow.CamPosCount)
        {
            case 0:
                {
                    FPSView();
                    break;
                }
            case 1:
                {
                    TPSView();
                    break;
                }
            case 2:
                {
                    QuarterView();
                    break;
                }
        }
     
    }

    private void FPSView()
    {
        // 1. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90.0f, 90.0f);

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void TPSView()
    {
        return;
    }

    private void QuarterView()
    {
        return;
    }

}
