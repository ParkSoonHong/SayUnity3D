using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance = null;

    public Slider StaminaSlider;

    public GameObject CompositekeyText;

    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI BombText;

    private void Awake()
    {
        if(Instance == null)
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

    public void UpdateBullet(int currentCount,int maxCount)
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
}
