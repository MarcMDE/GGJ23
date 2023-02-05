using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;

    void Start()
    {
        rb.velocity = transform.forward * -speed;
        rb.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.0f);
        if (gameObject != null)
            Destroy(gameObject);

    }
}
