using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemController : Singleton<DisplayItemController>
{
    private Dictionary<string, Sprite> _displayItems;      // 표시 아이템 딕셔너리
    
    // 베란다 대화 GUI - 표시 아이템
    [HideInInspector] public GameObject verandaDisplayItemPanelGameObject;            // 표시 아이템 패널 오브젝트
    [HideInInspector] public Image verandaDisplayItemImage;                           // 표시 아이템 이미지
    
    [HideInInspector] public bool isFading;
    
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
        
        Image dItem = verandaDisplayItemPanelGameObject.GetComponent<Image>();
        dItem.color = new Color(1f, 1f, 1f, 0f);
        isFading = true;
        dItem.DOFade(1f, 0.75f).OnComplete(() =>
        {
            isFading = false;
        });
    }

    public void HideDisplayItem()
    {
        Debug.Log("표시 아이템 숨기기!");
        
        isFading = true;
        verandaDisplayItemPanelGameObject.GetComponent<Image>().DOFade(0f, 0.75f).OnComplete(() =>
        {
            isFading = false;
            verandaDisplayItemPanelGameObject.SetActive(false);
            verandaDisplayItemPanelGameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        });
    }
}
