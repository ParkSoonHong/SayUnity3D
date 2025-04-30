using System.Collections;
using TMPro;
using UnityEngine;

public class UI_GameFlow : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public float readyDuration = 3f;
    public float overDuration = 2f;
    // 예시 조건: 실제 게임 오버 여부를 결정하는 변수
   // private bool GameIsOver => /* your game over condition */;
    void Start()
    {
        // 처음에는 준비 상태 코루틴 실행
        StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        // ==== Ready 상태 ====
        Time.timeScale = 0f;                   // 게임 완전 정지 
        statusText.text = "Ready";
        // 실시간 대기 (시간 배율 무시) :contentReference[oaicite:11]{index=11}
        yield return new WaitForSecondsRealtime(readyDuration);

        // ==== Run 상태 ====
        statusText.text = "Go!";
        yield return new WaitForSecondsRealtime(0.5f);   // 게임 시간 기준 대기
        Time.timeScale = 1f;                   // 게임 재개 
        statusText.gameObject.SetActive(false);
        /*
  

        // 대략적 예시: 게임 오버 트리거 대기
        yield return new WaitUntil(() => GameIsOver);

        // ==== Over 상태 ====
        Time.timeScale = 0f;                   // 다시 일시정지 :contentReference[oaicite:13]{index=13}
        statusText.text = "Game Over";
        yield return new WaitForSecondsRealtime(overDuration);
        */
        // 이후 리스타트 혹은 메인 메뉴 전환 로직
    }

 

}
