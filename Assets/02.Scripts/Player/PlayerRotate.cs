using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150;

    private float _rotationX = 0;

    private int _camPosCount = 0;

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
        
    }

    private void QuarterView()
    {
     
    }
}
