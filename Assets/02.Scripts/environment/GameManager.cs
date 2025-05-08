using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private EGameState _gameState = EGameState.Ready;
    public EGameState GameState => _gameState;

    public float WaitTime = 3;

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

        // 게임 오브젝트가 삭제될 경우 게임 오브젝트의 참조는 잃지만
        // 스태틱 변수가 남아 있어서 오류가 생기는 경우가 있다.
        // 이럴 경우에는 게임 오브젝트가 삭제되지 않도록
        DontDestroyOnLoad(gameObject); // -> 씬이 바뀌어도 '그 게임 오브젝트는 삭제하지 않겠다' 라는 의미
    }

    public void Start()
    {
        Ready();
    }

    public void Ready()
    {
        _gameState = EGameState.Ready;
        Time.timeScale = 0f;
        UI_GameFlow.Instance.UpdateReadyUI();
        StartCoroutine(Run());
    }

    public IEnumerator Run()
    {
        yield return new WaitForSecondsRealtime(WaitTime);
        Time.timeScale = 1f;
        UI_GameFlow.Instance.UpdateStartUI();
        _gameState = EGameState.Run;
        yield break;
    }

    public void Pause()
    {
        _gameState = EGameState.Pause;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;

        Ui_PopupManager.Instance.Open(EPopupType.Ui_OptionPopup, closeCallback: Continue);
    }

    public void Continue()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReStart()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // 다시시작을 했더니 게임이 망가지는 경우가 있다.
        // 싱글톤 처리를 잘못했을경우 망가진다.
    }
}
