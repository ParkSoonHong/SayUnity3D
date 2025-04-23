using UnityEngine;

public class TracerBullet : MonoBehaviour
{

    public float Speed = 5000;

    private void Update()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }

}
