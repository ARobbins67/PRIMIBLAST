using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WeightController : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    private Rigidbody2D rb;
    private bool bIsCoupled = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bIsCoupled)
            {
                OnDecouple.Invoke();
                bIsCoupled = false;
            }
            else if (!bIsCoupled)
            {
                OnCouple.Invoke();
                bIsCoupled = true;
            }
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Wall")
        {
            particles.transform.position = collision.transform.position;
            particles.Play();
        }
    }
}
