using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float Speed = 4f;
    [SerializeField] float StartTorque = 5f;
    // [SerializeField] int ScoreValue = 50;
    [SerializeField] Vector2 StartForce;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 startForce = transform.up * Speed;

        Quaternion randRotation = Random.rotation;
        randRotation.x = 0;
        randRotation.y = 0;
        transform.rotation = randRotation;

        rb.AddForce(startForce, ForceMode2D.Impulse);
        rb.AddTorque(StartTorque);
    }
}
