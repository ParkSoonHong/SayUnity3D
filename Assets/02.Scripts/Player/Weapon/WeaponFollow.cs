using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public GameObject WeaponPosition;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = WeaponPosition.transform.position;
        transform.rotation = WeaponPosition.transform.rotation;
    }
}
