using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationController : Singleton<IllustrationController>
{
    private Dictionary<string, Sprite> _illustrations;    // 삽화 딕셔너리

    // 베란다 대화 GUI - 일러스트
    [HideInInspector] public GameObject verandaIllustrationGameObject;        // 일러스트 오브젝트
    [HideInInspector] public Image verandaIllustrationImage;                  // 일러스트 이미지
    
    // 방 대화 GUI - 화면 표시 이미지
    [HideInInspector] public GameObject roomDisplayImagePanelGameObject;      // 방 화면 표시 이미지 게임 오브젝트
    [HideInInspector] public Image roomDisplayImage;                          // 방 화면 표시 이미지 

    private void Start()
    {
        _illustrations = ResourceManager.Instance.LoadAll<Sprite>("Illustration");
    }

    public void ShowIllustration(string illustrationName, DialogueType dialogueType)
    {
        Debug.Log($"삽화 출력! >> {illustrationName}");
        Sprite illustration = _illustrations[illustrationName];
        if (!illustration)
        {
            Debug.LogError($"다음 이름의 삽화를 찾을 수 없습니다. >> {illustrationName}");
        }

        switch (dialogueType)
        {
            case DialogueType.RoomDialogue:
            case DialogueType.RoomNarration:
                roomDisplayImage.sprite = illustration;
                roomDisplayImagePanelGameObject.SetActive(true);
                break;
            
            case DialogueType.VerandaDialogue:
                verandaIllustrationImage.sprite = illustration;
                verandaIllustrationGameObject.SetActive(true);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없어 삽화를 출력할 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }
    }

    public void HideIllustration(DialogueType dialogueType)
    {
        Debug.Log("삽화 숨기기!");
        switch (dialogueType)
        {
            case DialogueType.RoomDialogue:
            case DialogueType.RoomNarration:
                roomDisplayImagePanelGameObject.SetActive(false);
                break;
            
            case DialogueType.VerandaDialogue:
                verandaIllustrationGameObject.SetActive(false);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없어 삽화를 숨길 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }
    }
}
