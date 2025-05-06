using System;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public Transform cameraTransform; 

    public float RotationSpeed = 150;

    private float _rotationX = 0;

    private int _camPosCount = 0;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _camPosCount = 0;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _camPosCount = 1;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            transform.rotation = Quaternion.Euler(new Vector3(45f, 45f, 0f));
            _camPosCount = 2;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
        }

        CameraPosition();
    }

    public void CameraPosition()
    {
        switch (_camPosCount)
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

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }

    private void TPSView()
    {
        // 1) 입력 축값을 가져와 벡터로 만든 뒤 정규화합니다.
        float h = Input.GetAxis("Horizontal");  
        float v = Input.GetAxis("Vertical");    
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // 2) 입력이 일정 값 이상일 때만 회전하도록 조건을 겁니다.
        if (inputDir.magnitude >= 0.1f)
        {
            // 3) 입력 벡터의 각도를 라디안→도 단위로 변환합니다.
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg; 

            // 혹시 카메라 기준 방향으로 회전시키고 싶으면 카메라의 Y 회전값을 더해줍니다.
            if (cameraTransform != null)
                targetAngle += cameraTransform.eulerAngles.y;  

            // 4) Quaternion.Euler을 사용해 Y축 회전을 바로 적용합니다.
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); 
        }
    }

    private void QuarterView()
    {
     
    }
}
