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

    private void OnEnable()
    {
        GameManager.instance.OnBattle += StartAction;
    }

    private void OnDisable()
    {
        GameManager.instance.OnBattle -= StartAction;
    }

    private void StartAction()
    {
        if(gameObject.activeSelf)
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
