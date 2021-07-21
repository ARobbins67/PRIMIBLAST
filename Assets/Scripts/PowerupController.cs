using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] float RapidFireDuration = 3f;
    [SerializeField] float RapidFireDelay = .2f;
    [SerializeField] float RapidFireForce = 2f;
    private WomboComboController wcCon;

    // Start is called before the first frame update
    void OnEnable()
    {
        wcCon = FindObjectOfType<WomboComboController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            wcCon.ActivateRapidFire(RapidFireDuration, RapidFireDelay, RapidFireForce);
            Destroy(gameObject);
        }
        else if(collision.collider.tag == "Enemy" || collision.collider.tag == "Projectile")
        {
            Destroy(gameObject);
            Destroy(collision.collider.gameObject);
        }
    }
}
