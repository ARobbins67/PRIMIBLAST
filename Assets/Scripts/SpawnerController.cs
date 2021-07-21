using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerController : MonoBehaviour
{
    [Header("Burst Settings")]
    [SerializeField] float DelayBetweenBurst = 10f;
    [SerializeField] int NumBurstEnemies = 3;

    [Header("Spawner Properties")]
    [SerializeField] float MoveSpeed = 3f;
    [SerializeField] float StoppingDistance = .5f;
    [SerializeField] [Tooltip("Out of 100")] float ChanceForPowerup = 10f;
    [SerializeField] float SpawnDelay = .5f;

    [Header("References")]
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Health;
    [SerializeField] List<GameObject> PatrolPoints = new List<GameObject>();
    [SerializeField] GameObject LeftBurstBound;
    [SerializeField] GameObject RightBurstBound;
    [SerializeField] GameObject UpperBurstBound;
    [SerializeField] GameObject LowerBurstBound;

    private GameObject destination;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        destination = PatrolPoints[i];
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        Vector2 direction = (destination.transform.position-transform.position).normalized;
        Vector3 newPos = Vector3.zero;
        newPos.x = transform.position.x + (direction.x*.1f*MoveSpeed);
        newPos.y = transform.position.y + (direction.y*.1f*MoveSpeed);

        transform.position = newPos;
        if(Vector2.Distance(transform.position, destination.transform.position)<StoppingDistance)
        {
            i++;
            if(i > PatrolPoints.Count-1)
            {
                i = 0;
            }
            destination = PatrolPoints[i];
        }
    }

    public void StartSpawn()
    {
        StartCoroutine("Spawn");
        StartCoroutine("Burst");
    }

    private IEnumerator Burst()
    {
        while (true)
        {
            yield return new WaitForSeconds(DelayBetweenBurst);
            NumBurstEnemies++;
            for(int i = 0; i < NumBurstEnemies; i++)
            {
                Vector2 randPos;
                randPos.x = Random.Range(LeftBurstBound.transform.position.x, RightBurstBound.transform.position.x);
                randPos.y = Random.Range(LowerBurstBound.transform.position.y, UpperBurstBound.transform.position.y);

                Instantiate(Enemy, randPos, Quaternion.identity);
            }
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            float num = Random.Range(0,100);
            if(num < ChanceForPowerup)
            {
                Instantiate(Health, transform.position, Quaternion.identity, null);
            }
            else
            {
                Instantiate(Enemy, transform.position, Quaternion.identity, null);
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
}
