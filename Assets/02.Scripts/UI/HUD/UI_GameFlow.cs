using System.Collections;
using TMPro;
using UnityEngine;

public class UI_GameFlow : MonoBehaviour
{

    private static UI_GameFlow _instance = null;
    public static UI_GameFlow Instance => _instance;

    public TextMeshProUGUI statusText;
    public float readyDuration = 3f;
    public float overDuration = 2f;


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

    public void UpdateReadyUI()
    {
        statusText.text = "Ready";
    }    

    public void UpdateStartUI()
    {
        statusText.text = "Go!";
        GameStart();
    }

    public void GameStart()
    {
        StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        statusText.gameObject.SetActive(false);

        yield break;
    }

 

}
