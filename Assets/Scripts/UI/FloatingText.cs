using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float DestroyTime = 3f;
    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }


}
