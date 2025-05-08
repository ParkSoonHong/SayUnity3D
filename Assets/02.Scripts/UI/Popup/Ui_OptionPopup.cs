using NUnit.Framework;
using UnityEngine;

public class Ui_OptionPopup : Ui_Popup
{

    public void OnClickContinueButton()
    {
        GameManager.Instance.Continue();

        gameObject.SetActive(false);
    }

    public void OnClickRetryButton()
    {
        GameManager.Instance.ReStart();
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickCreaditButton()
    {
        Ui_PopupManager.Instance.Open(EPopupType.Ui_CreditPopup);
    }
}
