using System;
using UnityEngine;

public class Ui_Popup : MonoBehaviour
{
    // 콜백 함수 : 어떤 함수를 기억해놨다가 특정 시점(작업이 완료된 후) 호출하는 함수
    private Action _cloaseCallback;
    public void Open(Action cloaseCallback = null)  // 1. 야! 팝업열려! 근데! 너 닫힐때 호출할 함수좀 등록할게
    {
        _cloaseCallback = cloaseCallback;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _cloaseCallback?.Invoke();                  // 2. 나. 닫힌다? 어?! 닫힐때 호출할 함수등록 되어있네 실행.
        gameObject.SetActive(false);

        // 옵션 팝업일 경우에는 GamaManager -> Continue 호출
    }

}
