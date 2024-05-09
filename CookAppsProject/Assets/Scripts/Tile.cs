using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public Unit Unit;
    [HideInInspector]
    public int x, y;
    private Image _image;
    private const float ToggleColorTime = 0.5f;
   
    public void Init(int x, int y)
    {
        Unit = null;
        this.x = x;
        this.y = y;
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// null -> true, not null -> false
    /// </summary>
    /// <returns></returns>
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
    }

    public void ChangeColor(bool isMine)
    {
        StartCoroutine(ToggleColor(isMine));
    }

    IEnumerator ToggleColor(bool isMine)
    {
        _image.color = isMine ? Color.blue : Color.red;
        yield return new WaitForSecondsRealtime(ToggleColorTime);
        _image.color = Color.white;
    }
}
