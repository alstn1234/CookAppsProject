using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Tile ParentTile;

    #region Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시 실행 할 코드

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
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
                    SetParentTile(tile);
                    isChangeTile = true;
                    break;
                }
            }
        }

        // 옮길 수 없을 때
        if(!isChangeTile)
            transform.position = ParentTile.transform.position;
    }
    #endregion

    private void SetParentTile(Tile newParent)
    {
        ParentTile.DeleteUnit();
        newParent.SetUnit(this);
        gameObject.transform.position = newParent.transform.position;
    }
}
