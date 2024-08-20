using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitController : Singleton<PortraitController>
{
    private Dictionary<string, Sprite> _portraits;    // 초상화 딕셔너리
    
    // 베란다 대화 GUI - 초상화 
    [HideInInspector] public GameObject verandaPortraitGameObject;        // 초상화 오브젝트
    [HideInInspector] public Image verandaPortraitImage;                  // 초상화 이미지
    private void Start()
    {
        _portraits = ResourceManager.Instance.LoadAll<Sprite>("Portrait");
    }

    public void ShowPortrait(string portraitName, DialogueType dialogueType)
    {
        Debug.Log($"초상화 출력! >> {portraitName}");
        Sprite portrait = _portraits[portraitName];
        if (!portrait)
        {
            Debug.LogError($"다음 이름의 초상화를 찾을 수 없습니다. >> {portraitName}");
        }

        switch (dialogueType)
        {
            case DialogueType.VerandaDialogue:
                verandaPortraitImage.sprite = portrait;
                verandaPortraitGameObject.SetActive(true);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없거나 씬 타입에 맞는 UI가 없어 초상화를 출력할 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }
    }

    public void HidePortrait(DialogueType dialogueType)
    {
        Debug.Log("초상화 숨기기!");
        switch (dialogueType)
        {
            case DialogueType.VerandaDialogue:
                verandaPortraitGameObject.SetActive(false);
                break;

            default:
                Debug.LogError($"씬 타입을 식별할 수 없거나 씬 타입에 맞는 UI가 없어 초상화를 숨길 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }
    }
}
