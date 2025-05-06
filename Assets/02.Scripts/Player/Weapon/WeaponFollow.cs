using UnityEngine;

public class WeaponFollow : MonoBehaviour
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
