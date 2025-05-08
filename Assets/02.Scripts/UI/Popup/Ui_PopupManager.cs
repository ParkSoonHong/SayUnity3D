using UnityEngine;
using System.Collections.Generic;
using System;


public enum EPopupType
{
    Ui_CreditPopup,
    Ui_OptionPopup,
}

public class Ui_PopupManager : MonoBehaviour
{
    private static Ui_PopupManager _instance = null;
    public static Ui_PopupManager Instance => _instance;

    [Header("팝업 매니저 등록")]
    public List<Ui_Popup> Popups; // 모든 팝업을 관리


    private Stack<Ui_Popup> _openedPopups = new Stack<Ui_Popup>(); // 모든 팝업을 관리
    // 1. 
    // 2.

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_openedPopups.Count > 0)
            { 
                while(true)
                {
                    Ui_Popup popup = _openedPopups.Pop();

                    bool opend = popup.isActiveAndEnabled;
                    popup.Close();
              
                    if(opend || _openedPopups.Peek() == null)
                    {
                        break;
                    }
                }
              
            }
            else
            {
                GameManager.Instance.Pause();
            }
           // _openedPopups[^1].Close(); 위와 같음
        }
    }

    public void Open(EPopupType type, Action closeCallback = null)
    {
        Open(type.ToString(), closeCallback);
    }

    private void Open(string popupName, Action closeCallback)
    {
        foreach(Ui_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open();
                _openedPopups.Push(popup);
                break;
            }
        }
    }
}
