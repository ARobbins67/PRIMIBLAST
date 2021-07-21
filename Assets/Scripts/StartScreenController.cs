using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] float WaitTime = .7f;
    [SerializeField] UnityEvent OnBeforeCountdown;
    [SerializeField] UnityEvent OnAfterCountdown;

    private TextMeshProUGUI text;
    private int count = 3;

    void Awake()
    {
        Time.timeScale = 1;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = count.ToString();
        count = 3;
        OnBeforeCountdown.Invoke();
        StartCoroutine("Countdown");
    }

    private IEnumerator Countdown()
    {
        while (count >= -1)
        {
            if (count == 0)
            {
                text.text = "GO!";
            }
            else if(count < 0)
            {
                OnAfterCountdown.Invoke();
                gameObject.SetActive(false);
                StopCoroutine("Countdown");
            }
            else
            {
                text.text = count.ToString();
            }
            count--;
            yield return new WaitForSeconds(WaitTime);
        }
    }
}