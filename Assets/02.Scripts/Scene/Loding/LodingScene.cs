using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingScene : MonoBehaviour
{
    // 목표 : 다음 씬을 '비동기 방식'으로 로드하고 싶다.
    //       또한 로딩 진행률을 시각적으로 표현하고 싶다.
    //                           ㄴ % 프로그래스 바와 %별 텍스트

    // 속성:
    // - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;

    // - 프로그래스 슬라이더바
    public Slider ProgresSlider;

    // - 프로그래스 텍스트
    public TextMeshProUGUI ProgressText;


    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정돈 씬을 비동기로 로드한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while(ao.isDone == false)
        {
            // 비동기로 실행할 코드들
           // Debug.Log(ao.progress); // 0~1
            ProgresSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";

            // 서버와 통신헤서 유저 데이터나 기획 데이터를 받아오면 된다.

            /*
             * | 퍼센트  | 문구 예시                   |
| ---- | ----------------------- |
| 0%   | "숨을 고르고… 집중하라."         |
| 20%  | "물의 호흡, 제1형… 자세를 다잡는다." |
| 40%  | "검은 날이 진동한다. 결의가 깃든다."  |
| 60%  | "악귀의 기운이 감지된다… 준비하라."   |
| 80%  | "형제의 약속을 잊지 마라."        |
| 100% | "모든 준비는 끝났다. 나아갈 시간이다." |

             */

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

           // yield return new WaitForSeconds(1);// 초대기
            yield return null; // 1프레임 대기 
        }

    }
}
