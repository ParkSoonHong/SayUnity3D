using System;
using UnityEngine;

public class PlayerRotate 
{
    public Transform cameraTransform; 

    public float RotationSpeed = 250;

    private float _rotationX = 0;

    public float turnSmoothTime = 0.1f;

    private Player _player;
    public PlayerRotate(Player player)
    {
        _player = player;
        Initialized();
    }

    private void Initialized()
    {
        cameraTransform = Camera.main.transform;
    }

    public void Rotate()
    {
        switch (CameraModeManager.Instance.CurrentMode)
        {
            case CameraMode.FPS:
                {
                    FPSView();
                    break;
                }
            case CameraMode.TPS:
                {
                    TPSView();
                    break;
                }
            case CameraMode.QUARTER:
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

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _player.transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
    float turnSmoothVelocity;
    private void TPSView()
    {
        // 1) 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // 2) 입력이 없으면 아무 것도 하지 않음
        if (inputDir.magnitude < 0.1f)
            return;

        // 3) 카메라 기준 이동 방향 계산
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camRight * inputDir.x + camForward * inputDir.z;

        // 4) 목표 회전 각도 계산 (카메라 기준)
        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        // 5) 부드러운 회전
        float angle = Mathf.SmoothDampAngle(
            _player.transform.eulerAngles.y,
            targetAngle,
            ref turnSmoothVelocity,
            turnSmoothTime
        );
        _player.transform.rotation = Quaternion.Euler(0f, angle, 0f);  // 캐릭터 회전 :contentReference[oaicite:1]{index=1}

    }

    private void QuarterView()
    {
       // transform.rotation = Quaternion.Euler(new Vector3(45f, 45f, 0f));
    }
}
