using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueSceneHandler : MonoBehaviour, ISceneInitializer
{
    // 베란다 대화 GUI
    // 베란다 대화 GUI - 대화
    public GameObject verandaDialoguePanelGameObject;       // 대화 패널 오브젝트
    public Image verandaCharacterPortraitImage;             // 캐릭터 초상화 이미지
    public TMP_Text verandaDialogueText;                    // 대화 텍스트
    // 베란다 대화 GUI - 독백
    public GameObject verandaMonologuePanelGameObject;      // 독백 패널 오브젝트
    public TMP_Text verandaMonologueText;                   // 독백 텍스트
    
    // 베란다 대화 GUI - 일러스트
    public GameObject verandaIllustrationGameObject;        // 일러스트 오브젝트
    public Image verandaIllustrationImage;                  // 일러스트 이미지
    
    // 베란다 대화 GUI - 표시 아이템
    public GameObject verandaDisplayItemPanelGameObject;    // 표시 아이템 패널 오브젝트
    public Image verandaDisplayItemImage;                   // 표시 아이템 이미지
    
    // 베란다 대화 GUI - 선택지
    public GameObject verandaChoicePanelGameObject;         // 선택지 패널 오브젝트
    public List<Button> verandaChoiceButtons;               // 선택지 버튼 리스트
    
    private void Start()
    {
        InitializeScene();
        
        // 해당 날짜의 대화 시작 메서드를 페이드아웃 종료시 실행되는 이벤트에 구독 
        SceneController.Instance.OnFadeComplate += StartTodayDialogue;
    }
    
    public void InitializeScene()
    {
        // 대화 씬 매니저 UI 초기화
        DialogueManager.Instance.verandaDialoguePanelGameObject = verandaDialoguePanelGameObject;
        DialogueManager.Instance.verandaCharacterPortraitImage = verandaCharacterPortraitImage;
        DialogueManager.Instance.verandaDialogueText = verandaDialogueText;
        DialogueManager.Instance.verandaMonologuePanelGameObject = verandaMonologuePanelGameObject;
        DialogueManager.Instance.verandaMonologueText = verandaMonologueText;

        // 일러스트 컨트롤러 UI 초기화
        DialogueManager.Instance.illustrationController.verandaIllustrationGameObject = verandaIllustrationGameObject;
        DialogueManager.Instance.illustrationController.verandaIllustrationImage = verandaIllustrationImage;
        
        // 표시 아이템 컨트롤러 UI 초기화
        DialogueManager.Instance.displayItemController.verandaDisplayItemPanelGameObject = verandaDisplayItemPanelGameObject;
        DialogueManager.Instance.displayItemController.verandaDisplayItemImage = verandaDisplayItemImage;

        // 선택지 컨트롤러 UI 초기화
        DialogueManager.Instance.choiceController.verandaChoicePanelGameObject = verandaChoicePanelGameObject;
        DialogueManager.Instance.choiceController.verandaChoiceButtons = verandaChoiceButtons;
    }

    private void StartTodayDialogue()
    {
        DialogueManager.Instance.StartDialogue(GameManager.Instance.dates.Find(date => date.date == GameManager.Instance.Date).linkedDialogueSceneID);
    }
}
