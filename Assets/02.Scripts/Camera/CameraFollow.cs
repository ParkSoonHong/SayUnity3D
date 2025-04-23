using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform FPSCamPOS;
    public Transform TPSCamPOS;
    public Transform QuarterCamPOS;

    private int _CamPosCOunt = 0;

   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _CamPosCOunt = 0;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
       
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _CamPosCOunt = 1;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
        
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _CamPosCOunt = 2;
            // 보간 기법 interpoling, smooting 기법이 들어갈 예정
         
        }

        CameraPosition();
    }

    public void CameraPosition()
    {
        switch(_CamPosCOunt)
        {
            case 0:
                {
                    transform.position = FPSCamPOS.position;
                    break;
                }
            case 1:
                {
                    transform.position = TPSCamPOS.position;
                    break;
                }
            case 2:
                {
                    transform.position = QuarterCamPOS.position;
                    break;
                }
        }
    }
}
