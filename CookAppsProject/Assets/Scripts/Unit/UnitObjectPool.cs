using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjectPool : MonoBehaviour
{
    public static UnitObjectPool instance;

    Dictionary<string, GameObject> UnitPrefab;
    Dictionary<string, Queue<GameObject>> Pool;

    private const int _poolCount = 5;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UnitPrefab = new Dictionary<string, GameObject>();
        Pool = new Dictionary<string, Queue<GameObject>>();
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (var unit in Resources.LoadAll<GameObject>("Prefabs/Unit"))
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            UnitPrefab[unit.name] = unit;
            for (int i = 0; i < _poolCount; i++)
            {
                var unitObj = CreateObject(unit.name);
                pool.Enqueue(unitObj);
            }
            Pool[unit.name] = pool;
        }
    }

    private GameObject CreateObject(string unitName)
    {
        if (!UnitPrefab.ContainsKey(unitName))
        {
            Debug.Log("존재하지 않는 이름");
            return null;
        }
        var unitObj = Instantiate(UnitPrefab[unitName], transform);
        unitObj.name = unitName;
        unitObj.SetActive(false);
        return unitObj;
    }

    public Unit Pop(string unitName)
    {
        if (!Pool.ContainsKey(unitName))
        {
            Debug.Log("존재하지 않는 이름");
            return null;
        }
        var unitPool = Pool[unitName];
        var unitObj = unitPool.Dequeue();
        unitObj.GetComponent<UnitHp>().ResetHp();
        unitObj.SetActive(true);
        unitObj.GetComponent<Unit>().ParentTile = null;
        return unitObj.GetComponent<Unit>();
    }

    public void Push(Unit unit)
    {
        unit.ParentTile.DeleteUnit();
        unit.FlipX(1);
        unit.gameObject.SetActive(false);
        Pool[unit.name].Enqueue(unit.gameObject);
    }

    public void ResetPool()
    {
        foreach(var unit in GameManager.instance.MyUnits)
        {
            Push(unit);
        }

        foreach (var unit in GameManager.instance.EnemyUnits)
        {
            Push(unit);
        }
    }
}
