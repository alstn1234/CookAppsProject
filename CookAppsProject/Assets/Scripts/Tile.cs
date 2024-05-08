using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public Unit Unit;
    private int x, y;
   
    public void Init(int x, int y)
    {
        Unit = null;
        this.x = x;
        this.y = y;
    }

    public bool CheckUnit()
    {
        if (Unit != null) return false;
        return true;
    }

    public void DeleteUnit()
    {
        Unit = null;
    }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        unit.ParentTile = this;
    }
}
