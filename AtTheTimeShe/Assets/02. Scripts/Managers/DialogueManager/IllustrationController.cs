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
    [HideInInspector] public Animator verandaIllustrationAnimator;            // 베란다 일러스트 애니메이터  

    // 방 대화 GUI - 화면 표시 이미지
    [HideInInspector] public GameObject roomDisplayImagePanelGameObject;      // 방 화면 표시 이미지 게임 오브젝트
    [HideInInspector] public Animator roomIllustrationAnimator;               // 방 일러스트 애니메이터 

    public void ShowAnimationIllustration(string illustrationName, DialogueType dialogueType)
    {
        Debug.Log($"삽화 출력! >> {illustrationName}");
        switch (dialogueType)
        {
            case DialogueType.RoomDialogue:
            case DialogueType.RoomNarration:
                roomDisplayImagePanelGameObject.SetActive(true);
                roomIllustrationAnimator.Play(illustrationName);
                break;
            
            case DialogueType.VerandaDialogue:
                verandaIllustrationGameObject.SetActive(true);
                verandaIllustrationAnimator.Play(illustrationName);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없어 삽화 애니메이션을 출력할 수 없습니다. 씬 타입 : {dialogueType}");
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
