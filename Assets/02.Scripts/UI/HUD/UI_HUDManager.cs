using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUDManager : MonoBehaviour
{
    public static UI_HUDManager Instance = null;

    public Slider StaminaSlider;
    public Slider HealthSlider;
    public Image HitEffectImage;
    public float HitTime = 1f;

    private Coroutine _hitCoroutine;

    public GameObject CompositekeyText;

    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI BombText;

    public List<Image> IconList;
    public Image Crosshair;
    private void Awake()
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

    public void ReLodingText()
    {
        BulletText.text = $"RELOADING";
    }

    public void UpdateBullet(int currentCount, int maxCount)
    {
        BulletText.text = $"장탄수: {currentCount} / {maxCount}";

    }

    public void UpdateBomb(int currentCount, int maxCount)
    {
        BombText.text = $"폭탄: {currentCount} / {maxCount}";
    }
    public void UpdatePlayerStamina(float stamina)
    {
        StaminaSlider.value = stamina;
    }

    public void UpdateCompositekeyText(bool chek)
    {
        CompositekeyText.SetActive(chek);
    }

    public void UpdateHealth(float health)
    {
        HealthSlider.value = health;

        if (_hitCoroutine != null) return;

        _hitCoroutine = StartCoroutine(HitEffect());
    }

    public void UpdateSwap(ECharacterType characterType)
    {
        if (characterType == ECharacterType.Tanjiro)
        {
            Crosshair.gameObject.SetActive(false);
        }
        else
        {
            Crosshair.gameObject.SetActive(true);
        }

        for (int i = 0; i < (int)ECharacterType.Count; i++)
        {
            if (i == (int)characterType)
            {
                IconList[i].gameObject.SetActive(true);
                continue;
            }
            IconList[i].gameObject.SetActive(false);
        }
    }
    private IEnumerator HitEffect()
    {
        float elapsed = 0f;
        Color original = HitEffectImage.color;
        original.a = 1f;
        HitEffectImage.color = original;
        while (elapsed < HitTime)
        {
            // 경과 시간 누적
            elapsed += Time.deltaTime;
            // 보간 비율 0~1 계산
            float t = Mathf.Clamp01(elapsed / HitTime);
            // 알파값 1 → 0 보간
            Color c = original;
            c.a = Mathf.Lerp(1f, 0f, t);
            HitEffectImage.color = c;

            yield return null;  // 다음 프레임까지 대기
        }

        // 완전히 투명하게 마무리
        Color end = original;
        end.a = 0f;
        HitEffectImage.color = end;

        // 재실행 가능하도록 코루틴 참조 해제
        _hitCoroutine = null;
    }
}
