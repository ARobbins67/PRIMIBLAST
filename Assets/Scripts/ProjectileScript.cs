using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] float StartForce = 15f;
    [SerializeField] int ScoreAmount = 10;
    [Tooltip("Number of non-weight hits before projectile is destroyed")]
    [SerializeField] int HitCount;

    [SerializeField] GameObject DebugUI;

    private WomboComboController wcCon;
    private Rigidbody2D rb;
    private int hits = 0;
    Vector3 _origPos;
    private EndScreenController[] tempArray;
    private EndScreenController endScreenCon;

    private void OnEnable()
    {
        tempArray = Resources.FindObjectsOfTypeAll<EndScreenController>();
        endScreenCon = tempArray[0];
        wcCon = FindObjectOfType<WomboComboController>();
        rb = GetComponent<Rigidbody2D>();
        endScreenCon = FindObjectOfType<EndScreenController>();

        rb.AddForce(transform.up * StartForce, ForceMode2D.Impulse);
        _origPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hits++;
        if(collision.collider.gameObject != gameObject)
        {
            switch (collision.collider.tag)
            {
                case "Projectile":
                    Destroy(collision.collider.gameObject);
                    Destroy(gameObject);
                    break;
            }
            
        }
        if (hits >= HitCount)
        {
            Destroy(gameObject);
        }
    }
}
