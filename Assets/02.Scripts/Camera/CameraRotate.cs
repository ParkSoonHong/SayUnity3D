using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 있다.
    // 구현 순서

    public float RotationSpeed = 250;

    // 카메라 각도는 0도에서 부터 시작한다고 기준을 세운다.
    private float _rotationX = 0;
    private float _rotationY = 0;

    private float _minPitch = -90;
    private float _maxPitch = 90;

   
    private void Update()
    {
        switch (CameraModeManager.Instance.CurrentMode)
        {
            case CameraMode.FPS:
                {
                    if (CameraFollow.Instance.isEvent)
                    {
                        return;
                    }

                    FPSView();
                    break;
                }
            case CameraMode.TPS:
                {
                    TPSView();
                    break;
                }
            default:
                {
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
        _rotationY = Mathf.Clamp(_rotationY, _minPitch, _maxPitch);

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void TPSView()
    {
        if (CameraModeManager.Instance.CurrentMode != CameraMode.TPS)
            return;

        // 1) 마우스 입력
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2) 궤도 yaw·pitch 누적
        CameraFollow.Instance.Yaw += mouseX * RotationSpeed * Time.deltaTime;
        CameraFollow.Instance.Pitch = Mathf.Clamp(
            CameraFollow.Instance.Pitch - mouseY * RotationSpeed * Time.deltaTime,
            CameraFollow.Instance.MinPitch, CameraFollow.Instance.MaxPitch
        );

        // 3) 카메라 바로 회전(방향만)
        transform.rotation = Quaternion.Euler(CameraFollow.Instance.Pitch, CameraFollow.Instance.Yaw, 0f);
    }

    private void QuarterView()
    {
        return;
    }

}
