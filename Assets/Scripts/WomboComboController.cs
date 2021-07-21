using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WomboComboController : MonoBehaviour
{
    [SerializeField] LineRenderer lineRend;
    [SerializeField] float ForceToLengthRatio = 5f;
    [SerializeField] float WidthToLengthRatio = 2f;
    [SerializeField] float MaxSpeed = 10f;
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject EndScreenObj;
    private EndScreenController endScreenCon;

    private AudioSource audio;
    private GameObject triangle;
    private float rapidFireForce;
    private GameObject projectileSpawn;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector2 startPos;
    Vector3 worldPos; 
    private float forceMag = 0;
    private float timer = 0f;
    private float rapidFireDelay;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        endScreenCon = EndScreenObj.GetComponent<EndScreenController>();
        projectileSpawn = transform.Find("ProjectileSpawn").gameObject;
        triangle = lineRend.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        #if PLATFORM_ANDROID
        HandleTouchInput();
        #endif

        #if UNITY_EDITOR
        HandleMouseInput();
        #endif

        #if PLATFORM_IOS
        HandleTouchInput();
        #endif

        Vector2 vel = new Vector2();
        vel.x = Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed);
        vel.y = Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed);
        rb.velocity = vel;
    }

    public void ActivateRapidFire(float duration, float delay, float force)
    {
        timer = duration;
        rapidFireDelay = delay;
        rapidFireForce = force;
        StartCoroutine("RapidFire");
    }

    private void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        else if (timer <= 0)
        {
            StopCoroutine("RapidFire");
            timer = 0;
        }
    }

    private IEnumerator RapidFire()
    {
        while(timer >= 0)
        {
            Vector2 dir = -transform.up.normalized;

            rb.AddForce(dir*rapidFireForce, ForceMode2D.Impulse);
            audio.Play();
            Fire();
            yield return new WaitForSeconds(rapidFireDelay);
            if(timer <= 0)
            {
                break;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            Debug.DrawLine(transform.position, touchPosition, Color.red);
            
            if(touch.phase == TouchPhase.Began)
            {
                triangle.SetActive(true);
                Vector3 touchPos = touch.position;
                worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                worldPos.z = 0;
                lineRend.enabled = true;
                lineRend.SetPosition(0, worldPos);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                Vector3 endPos = Camera.main.ScreenToWorldPoint(touch.position);
                endPos.z = 0;
                lineRend.SetPosition(1, endPos);

                direction = (endPos - worldPos).normalized;

                Vector3 eulers = Vector3.zero;
                eulers.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                Quaternion quat = Quaternion.Euler(eulers);
                transform.SetPositionAndRotation(transform.position, quat);

                Vector2 midPoint = ((lineRend.GetPosition(1) + lineRend.GetPosition(0)) / 2);

                triangle.transform.SetPositionAndRotation(midPoint, quat);
                float length = Vector2.Distance(lineRend.GetPosition(1), lineRend.GetPosition(0));
                forceMag = length * ForceToLengthRatio;

                lineRend.widthMultiplier = length * .01f * ForceToLengthRatio;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                rb.AddForce(direction * forceMag, ForceMode2D.Impulse);
                triangle.SetActive(false);
                lineRend.enabled = false;
                audio.Play();

                Fire();
            }
        }
    }

    private void Fire()
    {
        Instantiate(Projectile, projectileSpawn.transform.position, transform.rotation);
    }
    
    private void HandleMouseInput()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            triangle.SetActive(true);
            Vector3 mousePos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            lineRend.enabled = true;
            lineRend.SetPosition(0, worldPos);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;
            lineRend.SetPosition(1, endPos);

            direction = (endPos - worldPos).normalized;

            Vector3 eulers = Vector3.zero;
            eulers.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Quaternion quat = Quaternion.Euler(eulers);
            transform.SetPositionAndRotation(transform.position, quat);

            Vector2 midPoint = ((lineRend.GetPosition(1) + lineRend.GetPosition(0)) / 2);
            
            triangle.transform.SetPositionAndRotation(midPoint, quat);
            float length = Vector2.Distance(lineRend.GetPosition(1), lineRend.GetPosition(0));
            forceMag = length * ForceToLengthRatio;

            lineRend.widthMultiplier = length*.01f*ForceToLengthRatio;
        }
        if (Input.GetMouseButtonUp(0))
        {
            rb.AddForce(direction * forceMag, ForceMode2D.Impulse);
            triangle.SetActive(false);
            lineRend.enabled = false;
            audio.Play();

            Fire();
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Projectile")
        {
            Destroy(collision.collider.gameObject);
            Time.timeScale = 0;
            endScreenCon.ShowEndScreen();
            audio.Stop();
            Destroy(gameObject);
        }
        else if (collision.collider.tag == "Enemy")
        {
            EnemyController enemyCon = collision.collider.gameObject.GetComponent<EnemyController>();
            if (enemyCon.canHurtPlayer())
            {
                Destroy(collision.collider.gameObject);
                Time.timeScale = 0;
                endScreenCon.ShowEndScreen();
                audio.Stop();
                Destroy(gameObject);
            }            
        }
    }
}
