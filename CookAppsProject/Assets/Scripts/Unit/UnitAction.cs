using System;
using System.Collections;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    private IUnitAction _unitAction;
    private UnitMovement _unitMovement;
    private void Awake()
    {
        _unitAction = GetComponent<IUnitAction>();
        _unitMovement = GetComponent<UnitMovement>();
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
        WaitForSeconds wfsr = new WaitForSeconds(1f);

        while (true)
        {
            if (_unitAction.CanAttack())
            {
                _unitAction.Attack();
            }
            else
            {
                _unitMovement.Move();
            }
            yield return wfsr;
        }
    }
}
