using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RoomSceneHandler : MonoBehaviour, ISceneInitializer
{
    [Header("Setting")] 
    public int goodChoiceNumberForNeatBedding;      // 정돈된 이불을 위한 좋은 선택 횟수
    public int goodChoiceNumberForCleanTrash;       // 쓰레기 청소를 위한 좋은 선택 횟수
    public List<int> randomMorningDialogueIDs;      // 아침 랜덤한 한 마디 대화 ID 리스트 
    public int day0MorningDialogueID;               // 0일차 아침 대화 ID 
    
    [Header("Reference")]
    public GameObject neatBeddingGameObject;        // 정돈된 이불 게임 오브젝트
    public GameObject noneNeatBeddingGameObject;    // 정돈되지 않은 이불 게임 오브젝트
    public GameObject trashGameObject;              // 쓰레기 봉지 게임 오브젝트
    public GameObject trashColliderGameObject;      // 쓰레기 봉지 콜라이더 게임 오브젝트 
    public GameObject darkPanelGameObject;          // 어두운 패널 게임 오브젝트 

    [Header("Initialize")] 
    public GameObject roomTextBoxPanelGameObject;   // 방 텍스트 박스 게임 오브젝트
    public TMP_Text roomTalkerText;                 // 방 텍스트 박스 화자 텍스트  
    public TMP_Text roomTextBoxText;                // 방 텍스트 박스 텍스트 

    public GameObject roomNarrationPanelGameObject; // 방 나레이션 패널 게임 오브젝트
    public TMP_Text roomNarrationText;              // 방 나레이션 패널 텍스트 
    
    public GameObject roomChoicePanelGameObject;    // 방 선택지 패널 게임 오브젝트 
    public List<Button> roomChoiceButtons;          // 방 선택지 버튼 리스트 
    
    public GameObject roomDisplayPanelGameObject;   // 방 표시 이미지 패널 게임 오브젝트
    public Animator roomDisplayAnimator;            // 방 표시 이미지 애니메이터 
    
    private void Start()
    {
        InitializeScene();
    }

    public void InitializeScene()
    {
        // 플레이어 상태 Room으로 변경. 
        GameManager.Instance.playerState = GameManager.PlayerState.Room;
        
        // 베란다에서 대화를 진행하였는지 확인
        // 대화 이후 ( 저녁 )
        if (GameManager.Instance.DidTodayDialogue)
        {
            
        }
        
        // 대화하기 이전 ( 아침 )
        else
        {
            // 방 상태 업데이트 
            UpdateRoomState();
            
            // Day 0인지 확인
            if (GameManager.Instance.Date == 0)
            {
                SceneController.Instance.OnFadeComplate +=
                    () => { DialogueManager.Instance.StartDialogue(day0MorningDialogueID); };
            }
            else
            {
                // 아침 랜덤 한 마디 이벤트에 등록 
                if (!GameManager.Instance.DidTodayDialogue)
                {
                    SceneController.Instance.OnFadeComplate += RandomMorningDialogue;
                }
            }
        }
        
        // 오늘 대화 여부에 따라 방 밝기 적용 
        darkPanelGameObject.SetActive(GameManager.Instance.DidTodayDialogue);
        
        // 대화 매니저 변수 할당
        DialogueManager.Instance.roomTextBoxPanelGameObject = roomTextBoxPanelGameObject;
        DialogueManager.Instance.roomTalkerText = roomTalkerText;
        DialogueManager.Instance.roomTextBoxText = roomTextBoxText;

        DialogueManager.Instance.roomNarrationTextPanelGameObject = roomNarrationPanelGameObject;
        DialogueManager.Instance.roomNarrationText = roomNarrationText;
        
        DialogueManager.Instance.choiceController.roomChoicePanelGameObject = roomChoicePanelGameObject;
        DialogueManager.Instance.choiceController.roomChoiceButtons = roomChoiceButtons;
        
        // 일러스트 컨트롤러 변수 할당 
        IllustrationController.Instance.roomDisplayImagePanelGameObject = roomDisplayPanelGameObject;
        IllustrationController.Instance.roomIllustrationAnimator = roomDisplayAnimator;
    }

    // 방 상태 업데이트 메서드 
    private void UpdateRoomState()
    {
        Debug.Log("방 정리!");
        int goodChoiceNumber = GameManager.Instance.GoodChoiceNumber;
        
        // 이불 정돈
        if (goodChoiceNumber >= goodChoiceNumberForNeatBedding)
        {
            noneNeatBeddingGameObject.SetActive(false);
            neatBeddingGameObject.SetActive(true);
        }
        
        // 쓰레기 청소 
        bool trashActive = !(goodChoiceNumber >= goodChoiceNumberForCleanTrash);
        trashGameObject.SetActive(trashActive);
        trashColliderGameObject.SetActive(trashActive);
    }

    private void RandomMorningDialogue()
    {
        int random = UnityEngine.Random.Range(0, randomMorningDialogueIDs.Count);
        DialogueManager.Instance.StartDialogue(randomMorningDialogueIDs[random]);
    }
}
