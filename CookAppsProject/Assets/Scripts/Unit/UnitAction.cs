using System;
using System.Collections;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    private IUnitAction _unitAction;
    private Unit _unit;
    private void Awake()
    {
        _unitAction = GetComponent<IUnitAction>();
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        GameManager.instance.OnBattle += StartAction;
    }

    private void OnDisable()
    {
        GameManager.instance.OnBattle -= StartAction;
    }

    private void StartAction()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(1f);

        while (true)
        {
            if (_unitAction.CanAttack())
            {
                _unitAction.Attack();
            }
            else
            {
                _unit.Move();
            }
            yield return wfsr;
        }
    }
}
