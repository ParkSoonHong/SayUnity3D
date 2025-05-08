using System;
using UnityEngine;
using static UnityEditor.SceneView;

public enum CameraMode
{ 
    FPS, 
    TPS, 
    QUARTER,
}
public class CameraModeManager : MonoBehaviour
{
    public static CameraModeManager Instance = null;
    public CameraMode CurrentMode = CameraMode.FPS;

    public delegate void ModeHendle(CameraMode cameraMode);

    public ModeHendle ModeHendleEvent;

    void Awake()
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CurrentMode = CameraMode.FPS;
            ModeHendleEvent?.Invoke(CurrentMode);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentMode = CameraMode.TPS;
            ModeHendleEvent?.Invoke(CurrentMode);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentMode = CameraMode.QUARTER;
            ModeHendleEvent?.Invoke(CurrentMode);
        }
    }
}
