using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float Speed = 5f;
    [SerializeField] float StartTorque;
    [SerializeField] int ScoreAmount = 50;
    [SerializeField] float FireDelay = .3f;
    [SerializeField] GameObject ProjectileSpawn;

    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject Health;

    private bool bCanHurtPlayer = false;
    private GameObject Turret;
    private EndScreenController[] tempArray;
    private EndScreenController endScreenCon;
    private Rigidbody2D rb;
    private ScoreController scoreCon;
    private CircleCollider2D col;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        scoreCon = FindObjectOfType<ScoreController>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        
        col.enabled = true;
        tempArray = Resources.FindObjectsOfTypeAll<EndScreenController>();
        endScreenCon = tempArray[0];

        Quaternion randRotation = Random.rotation;
        randRotation.x = 0;
        randRotation.y = 0;
        transform.rotation = randRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Weight" || collision.collider.tag == "Projectile")
        {
            ScoreController.ChangeScore(ScoreAmount);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rb.velocity = rb.velocity.normalized*Speed;
    }

    public void EndSpawn()
    {
        Vector2 startForce = transform.up * Speed;        

        rb.AddForce(startForce, ForceMode2D.Impulse);
        rb.AddTorque(StartTorque);
        bCanHurtPlayer = true;
        Turret = transform.GetChild(0).gameObject;
        StartCoroutine("Fire");
    }

    public bool canHurtPlayer()
    {
        return bCanHurtPlayer;
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(FireDelay);
            GameObject SpawnedProj = Instantiate(Projectile, ProjectileSpawn.transform.position, transform.rotation, null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position,transform.up);
    }
}
