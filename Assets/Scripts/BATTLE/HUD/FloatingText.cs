using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float DestroyTime = 3f; //duration before destroying the text object
    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }


}
