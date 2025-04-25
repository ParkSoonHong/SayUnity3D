using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour
{
    public Slider HealthSlider;

    public void UpdateHealth(float health)
    {
        HealthSlider.value = health;
    }

}
