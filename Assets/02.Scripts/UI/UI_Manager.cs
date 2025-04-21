using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance = null;

    public Slider StaminaSlider;

    public GameObject CompositekeyText;

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

    public void UpdatePlayerStamina(float stamina)
    {
        StaminaSlider.value = stamina;
    }

    public void UpdateCompositekeyText(bool chek)
    {
        CompositekeyText.SetActive(chek);
    }
}
