using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Unit _unit;
    private void Start()
    {
        _unit = GetComponent<Unit>();
    }

    #region Drag and Drop

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시 실행 할 코드

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.Mode == GameMode.Battle) return;
        transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.instance.Mode == GameMode.Battle) return;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        bool isChangeTile = false;
        foreach(var result in results)
        {
            var resultObj = result.gameObject;
            if(resultObj.layer == LayerMask.NameToLayer("Tile"))
            {
                var tile = resultObj.GetComponent<Tile>();
                if (tile.CheckUnit())
                {
                    _unit.ParentTile?.DeleteUnit();
                    _unit.SetParentTile(tile);
                    _unit.SetPosToParent();
                    isChangeTile = true;
                    break;
                }
            }
        }

        // 옮길 수 없을 때
        if(!isChangeTile)
            transform.position = _unit.ParentTile.transform.position;
    }
    #endregion


}
