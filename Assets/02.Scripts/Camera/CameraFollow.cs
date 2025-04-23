using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform FPSCamPOS;

    public float followSmooth = 0.2f;

    private int _camPosCount = 0;
    public int CamPosCount => _camPosCount;

    public Vector3 TPSViewOffset = new Vector3(0, 2, -4);
    public Vector3 QuarterViewOffset = new Vector3(0, 10, -10);
    public float moveSmooth = 0.1f;
  
    private Vector3 currentVelocity;
    public float rotateSpeed = 150f;

    private Vector3 velocityXZ;
    private float velocityY;

    private void LateUpdate()
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
        switch(_camPosCount)
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
        transform.position = FPSCamPOS.position;
    }

    public float smoothTime = 0.15f;             // 위치 부드러움
    public float minPitch = -30f, maxPitch = 60f; // 피치 제한

    private Vector3 velocity = Vector3.zero;     // 위치 보간용
    private float yaw = 0f, pitch = 10f;         // 누적 각도

    private void TPSView()
    {
        // 1) 마우스 입력
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");  // :contentReference[oaicite:8]{index=8}

        // 2) 누적 회전값 업데이트
        yaw += mouseX * rotateSpeed * Time.deltaTime;
        pitch -= mouseY * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // 피치 클램프 :contentReference[oaicite:9]{index=9}

        // 3) 사구면 좌표로 오프셋 회전
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = FPSCamPOS.position + rot * TPSViewOffset;

        // 4) 부드러운 위치 보간
        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPos,
            ref velocity, smoothTime    // :contentReference[oaicite:10]{index=10}
        );

        // 5) 캐릭터 바라보도록 회전
        Quaternion targetRot = Quaternion.LookRotation(
            FPSCamPOS.position - transform.position
        );                                // :contentReference[oaicite:11]{index=11}
        transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRot,
            rotateSpeed * Time.deltaTime
        );
    }


    private void QuarterView()
    {
        // 목표 위치: 플레이어 + offset
        Vector3 targetPos = FPSCamPOS.position + QuarterViewOffset;
        // 부드러운 위치 보간
        transform.position = Vector3.Lerp(
            transform.position, targetPos,
            followSmooth
        );
    }
}
