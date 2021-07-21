using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] float TimeScale;
    // Start is called before the first frame update
    void Start()
    {
        TimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
