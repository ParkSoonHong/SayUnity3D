using UnityEngine;

public class UI_MinimapButton : MonoBehaviour
{
    public Camera MinimapCamera;

    public float MaxOffset = 15;
    public float MinOffset = 0;
    

    public void SizeUpButton()
    {

        if (MinimapCamera.orthographicSize <= MinOffset) return;
        MinimapCamera.orthographicSize -= 1;
    }

    public void SizeDownButton()
    {
        if (MinimapCamera.orthographicSize >= MaxOffset) return;
        MinimapCamera.orthographicSize += 1;
    }
}
