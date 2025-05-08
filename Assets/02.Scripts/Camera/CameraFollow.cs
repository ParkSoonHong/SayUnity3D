using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance = null;
    
    public Transform FPSCamPOS;
    public Transform TPSCamPOS;

    public float FollowSmooth = 0.2f;

    private int _camPosCount = 0;
    public int CamPosCount => _camPosCount;

    public Vector3 TPSViewOffset = new Vector3(0, 2, -4);
    public Vector3 QuarterViewOffset = new Vector3(0, 10, -10);
    public float MoveSmooth = 0.1f;
  
    public float RotateSpeed = 150f;
    public float SmoothTime = 0.15f;             // 위치 부드러움
    public float MinPitch = -30f;
    public float MaxPitch = 60f; // 피치 제한

    private Vector3 _velocity = Vector3.zero;     // 위치 보간용
    public float Yaw = 0f;
    public float Pitch = 10f;         // 누적 각도

    public bool isEvent = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Yaw = FPSCamPOS.rotation.y;
        Pitch = FPSCamPOS.rotation.x;
    }

    private void LateUpdate()
    {
        switch (CameraModeManager.Instance.CurrentMode)
        {
            case CameraMode.FPS:
                {
                    if (isEvent) return;

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
        transform.position = FPSCamPOS.position;
    }

    private void TPSView()
    {
        Quaternion orbitRot = Quaternion.Euler(Pitch, Yaw, 0f);
        transform.position = TPSCamPOS.position + orbitRot * TPSViewOffset;
    }


    private void QuarterView()
    {
        Vector3 target = FPSCamPOS.position + QuarterViewOffset;
        transform.position = Vector3.Lerp(transform.position, target, FollowSmooth);
    }
}
