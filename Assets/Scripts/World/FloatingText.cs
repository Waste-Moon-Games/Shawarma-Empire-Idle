using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float life = 1f;

    private void Update()
    {
        life -= Time.deltaTime;
        transform.position += Vector3.up * (Time.deltaTime * 1.2f);
        if (life <= 0) Destroy(gameObject);
    }
}
