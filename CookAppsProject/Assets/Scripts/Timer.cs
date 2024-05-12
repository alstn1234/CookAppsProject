using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _time;
    private IEnumerator coroutine;
    private void Start()
    {
        coroutine = StartTimer();
        ResetTimer();
        GameManager.instance.OnBattle += BattleTimer;
        GameManager.instance.OnRestart += ResetTimer;
    }

    private void OnDisable()
    {
        GameManager.instance.OnBattle -= BattleTimer;
        GameManager.instance.OnRestart -= ResetTimer;
    }

    private void BattleTimer()
    {
        StartCoroutine(coroutine);
    }

    IEnumerator StartTimer()
    {
        _time = 60f;
        while(_time > 0f)
        {
            _time -= Time.deltaTime;
            _timerText.text = string.Format("{0:N1}", _time);
            yield return null;
        }
        if(_time <= 0f)
        {
            GameManager.instance.OnEndBattle?.Invoke();
        }
    }

    private void ResetTimer()
    {
        StopCoroutine(coroutine);
        _time = 60f;
        _timerText.text = string.Format("{0:N1}", _time);
    }
}
