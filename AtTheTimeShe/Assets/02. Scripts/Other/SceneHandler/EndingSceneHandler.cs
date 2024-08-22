using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingSceneHandler : MonoBehaviour
{
    // 베란다 대화 GUI
    // 베란다 대화 GUI - 대화
    public GameObject verandaDialoguePanelGameObject;       // 대화 패널 게임 오브젝트
    public TMP_Text verandaTalkerText;                      // 화자 텍스트 
    public TMP_Text verandaDialogueText;                    // 대화 텍스트
    
    // 베란다 대화 GUI - 나레이션 
    public GameObject verandaNarrationPanelGameObject;      // 나레이션 패널 게임 오브젝트
    public TMP_Text verandaNarrationText;                   // 나레이션 텍스트
    
    // 베란다 대화 GUI - 일러스트
    public GameObject verandaIllustrationGameObject;        // 일러스트 게임 오브젝트
    public Animator verandaIllustrationAnimator;            // 일러스트 애니메이터 
    
    // 베란다 대화 GUI - 초상화 
    public GameObject verandaPortraitGameObject;            // 초상화 게임 오브젝트
    public Image verandaPortraitImage;                      // 초상화 이미지
    
    // 베란다 대화 GUI - 표시 아이템
    public GameObject verandaDisplayItemPanelGameObject;    // 표시 아이템 패널 게임 오브젝트
    public Image verandaDisplayItemImage;                   // 표시 아이템 이미지
    
    // 베란다 대화 GUI - 선택지
    public GameObject verandaChoicePanelGameObject;         // 선택지 패널 게임 오브젝트
    public List<Button> verandaChoiceButtons;               // 선택지 버튼 리스트
    
    private void Start()
    {
        InitializeScene();
        
        // 엔딩 재생 메서드를 페이드아웃 종료시 실행되는 이벤트에 구독
        SceneController.Instance.OnFadeComplate += 
            () => { GameManager.Instance.PlayEnding(GameManager.Instance.SelectedEnding); };
    }
    
    public void InitializeScene()
    {
        // 대화 씬 매니저 UI 초기화
        // 대화
        DialogueManager.Instance.verandaDialoguePanelGameObject = verandaDialoguePanelGameObject;
        DialogueManager.Instance.verandaTalkerText = verandaTalkerText;
        DialogueManager.Instance.verandaDialogueText = verandaDialogueText;
        // 나레이션
        DialogueManager.Instance.verandaNarrationPanelGameObject = verandaNarrationPanelGameObject;
        DialogueManager.Instance.verandaNarrationText = verandaNarrationText;

        // 일러스트 컨트롤러 UI 초기화
        DialogueManager.Instance.illustrationController.verandaIllustrationGameObject = verandaIllustrationGameObject;
        DialogueManager.Instance.illustrationController.verandaIllustrationAnimator = verandaIllustrationAnimator; 
        
        // 초상화 컨트롤러 UI 초기화 
        PortraitController.Instance.verandaPortraitGameObject = verandaPortraitGameObject;
        PortraitController.Instance.verandaPortraitImage = verandaPortraitImage;
        
        // 표시 아이템 컨트롤러 UI 초기화
        DialogueManager.Instance.displayItemController.verandaDisplayItemPanelGameObject = verandaDisplayItemPanelGameObject;
        DialogueManager.Instance.displayItemController.verandaDisplayItemImage = verandaDisplayItemImage;

        // 선택지 컨트롤러 UI 초기화
        DialogueManager.Instance.choiceController.verandaChoicePanelGameObject = verandaChoicePanelGameObject;
        DialogueManager.Instance.choiceController.verandaChoiceButtons = verandaChoiceButtons;
    }
}
