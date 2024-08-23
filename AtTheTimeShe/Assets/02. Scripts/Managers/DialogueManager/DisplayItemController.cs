using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemController : Singleton<DisplayItemController>
{
    private Dictionary<string, Sprite> _displayItems;      // 표시 아이템 딕셔너리
    
    // 베란다 대화 GUI - 표시 아이템
    [HideInInspector] public GameObject verandaDisplayItemPanelGameObject;            // 표시 아이템 패널 오브젝트
    [HideInInspector] public Image verandaDisplayItemImage;                           // 표시 아이템 이미지
    
    private new void Awake()
    {
        base.Awake();
        
        _displayItems = ResourceManager.Instance.LoadAll<Sprite>("DisplayItem");
    }

    public void ShowDisplayItem(string displayItemName)
    {
        Debug.Log($"표시 아이템 출력! >> {displayItemName}");
        Sprite displayItem = _displayItems[displayItemName];
        if (!displayItem)
        {
            Debug.LogError($"다음 이름의 표시 아이템을 찾을 수 없습니다. >> {displayItem}");
        }
        verandaDisplayItemImage.sprite = displayItem;
        verandaDisplayItemPanelGameObject.SetActive(true);
    }

    public void HideDisplayItem()
    {
        Debug.Log("표시 아이템 숨기기!");
        verandaDisplayItemPanelGameObject.SetActive(false);
    }
}
