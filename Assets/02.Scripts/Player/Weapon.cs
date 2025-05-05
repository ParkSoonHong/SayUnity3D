using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject WeaponPosition;
    void Start()
    {
        transform.position = WeaponPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
