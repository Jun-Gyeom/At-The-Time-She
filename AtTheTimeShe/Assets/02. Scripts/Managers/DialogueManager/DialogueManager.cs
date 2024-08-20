using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// DialogueScene(대화 씬)들을 관리하는 클래스 
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private List<DialogueScene> _dialogueScenes;                              // 대화 씬 리스트
    
    public IllustrationController illustrationController;                     // 일러스트 컨트롤러
    public PortraitController portraitController;                             // 초상화 컨트롤러 
    public DisplayItemController displayItemController;                       // 표시 아이템 컨트롤러
    public ChoiceController choiceController;                                 // 선택지 컨트롤러
    
    // 베란다 대화창 GUI
    // 베란다 대화창 GUI - 대화 
    [HideInInspector] public GameObject verandaDialoguePanelGameObject;       // 대화 패널 오브젝트
    [HideInInspector] public TMP_Text verandaTalkerText;                      // 화자 텍스트 
    [HideInInspector] public TMP_Text verandaDialogueText;                    // 대화 텍스트
    // 베란다 대화창 GUI - 나레이션 
    [HideInInspector] public GameObject verandaNarrationPanelGameObject;      // 나레이션 패널 게임 오브젝트 
    [HideInInspector] public TMP_Text verandaNarrationText;                   // 나레이션 텍스트 
    
    // 방 대화창 GUI
    // 방 대화창 GUI - 대화 
    [HideInInspector] public GameObject roomTextBoxPanelGameObject;           // 방 텍스트 박스 게임 오브젝트
    [HideInInspector] public TMP_Text roomTalkerText;                         // 방 텍스트 박스 화자 텍스트 
    [HideInInspector] public TMP_Text roomTextBoxText;                        // 방 텍스트 박스 텍스트 
    // 방 대화창 GUI - 나레이션 
    [HideInInspector] public GameObject roomNarrationTextPanelGameObject;     // 방 나레이션 텍스트 패널 게임 오브젝트 
    [HideInInspector] public TMP_Text roomNarrationText;                      // 방 나레이션 텍스트
    
    private List<Dialogue> _currentDialogues;     // 현재 대화 리스트
    private int _currentDialogueID;               // 현재 대화 ID
    private int _currentScriptIndex;              // 현재 대사 인덱스
    private bool _hasChoice;                      // 현재 대화의 선택지 존재 여부 ( 존재한다면 다음 대화에 선택지 출력 )
    private int _choiceID;                        // 현재 대화의 선택지 ID
    private int _linkedDialogueID;                // 현재 대사의 연결된 대화 ID
    private bool _hasCharacterPortrait;           // 현재 대사의 초상화 존재 여부
    private DialogueType _dialogueType;           // 대화 씬 종류
    private IDialogueUI _dialogueUI;              // 현재 대사의 대화 UI 종류
    
    private void Start()
    {
        CSVParser csvParser = new CSVParser();
        
        // 대화 씬 데이터 파싱
        TextAsset dialoguesCSVFile = Resources.Load<TextAsset>("CSV/Dialogues");  // 경로 입력하기.
        _dialogueScenes = csvParser.LoadDialogues(dialoguesCSVFile);
    }
    
    // 대화 시작
    public void StartDialogue(int sceneID)
    {
        DialogueScene dialogueScene = _dialogueScenes.Find(dialogueScene => dialogueScene.sceneID == sceneID);
        if (dialogueScene == null) // 예외
        {
            Debug.LogError($"다음 ID를 가진 대화 씬이 존재하지 않습니다. : {sceneID}");
            return;
        }
        
        // 플레이어 상태 Dialogue로 전환
        GameManager.Instance.playerState = GameManager.PlayerState.Dialogue;
            
        _currentDialogues = dialogueScene.dialogues;    // 현재 대화 리스트 설정
        _currentDialogueID = 1;                         // 현재 대화 ID 초기화
        _currentScriptIndex = 0;                        // 현재 대사 인덱스 초기화
        _hasChoice = false;                             // 선택지 출력시킬지 여부 초기화
        
        NextDialogue();
    }

    // 다음 대화 실행
    public void NextDialogue(int dialogueID = -1)
    {
        if (_currentDialogues == null) // 대화 시작 여부
        {
            Debug.Log("대화 리스트가 비어있습니다.");
            return;
        }

        if (dialogueID == -1) // 파라미터 기본값 대입
        {
            dialogueID = _currentDialogueID;
        }
        else
        {
            _currentDialogueID = dialogueID;
        }

        if (choiceController.IsDisplayedChoice) // 현재 선택지 화면이 출력되어 있는 상태인지 검사
        {
            Debug.Log("선택지가 출력되어있습니다. 선택지를 선택하세요.");
            return;
        }
        
        // 대화 종료 시점 판별 로직 
        if (_currentDialogueID > _currentDialogues.Count) // 마지막 대화 ID보다 클 시
        {
            Debug.Log($"대화 타입 : {_dialogueType}");
            EndDialogue(); // 대화 종료
            return;
        }
        
        // 현재 대화와 대사 할당
        Dialogue dialogue = _currentDialogues.Find(dialogue => dialogue.dialogueID == dialogueID);
        if (dialogue == null) // 예외
        {
            Debug.LogError($"다음 ID를 가진 대화가 존재하지 않습니다. : {dialogueID}");
            return;
        }
        DialogueElement dialogueElement = dialogue.dialogueElements[_currentScriptIndex];
        
        // 대화 타입 대입 
        _dialogueType = dialogue.dialogueType;
        
        // 대사에 일러스트가 존재한다면 일러스트 출력
        if (HasIllustration(dialogueElement))
        {
            illustrationController.ShowIllustration(dialogueElement.illustrationName, _dialogueType);
        }
        
        // 대사에 초상화가 존재한다면 일러스트 출력
        if (HasCharacterPortrait(dialogueElement))
        {
            portraitController.ShowPortrait(dialogueElement.characterPortraitName, _dialogueType);
        }
        
        // 대사에 표시 아이템이 존재한다면 표시 아이템 출력
        if (HasDisplayItem(dialogueElement))
        {
            if (dialogueElement.displayItemName == "Delete Item")   // 표시 아이템 이름이 Delete Item이면 표시 아이템을 지우라는 의미이므로 표시 아이템 숨기기
            {
                displayItemController.HideDisplayItem();
            }
            else
            {
                displayItemController.ShowDisplayItem(dialogueElement.displayItemName);
            }
        }
        
        // 대사에 배경음악이 존재한다면 배경음악 재생
        if (HasBGM(dialogueElement))
        {
            if (dialogueElement.bgmName == "Stop Music")    // 배경음악 이름이 Stop Music이면 배경음악을 끄라는 의미이므로 배경음악 끄기
            {
                AudioManager.Instance.StopBGM();
            }
            else
            {
                AudioManager.Instance.PlayBGM(dialogueElement.bgmName);
            }
        }
        
        // 대사에 효과음이 존재한다면 효과음 재생
        if (HasSFX(dialogueElement))
        {
            AudioManager.Instance.PlaySFX(dialogueElement.sfxName);
        }
        
        _dialogueUI?.Hide(this); // 이전 대화 UI 숨기기
        
        if (_hasChoice) // 선택지 출력할지 여부 검사
        {
            // 선택지 출력 로직 
            choiceController.ShowChoice(_choiceID, _dialogueType, this);
            Debug.Log("선택지 출력!");
            
            _hasChoice = false;
        }
        else
        {
            // 대화 화면 출력 처리
            _dialogueUI = SelectDialogueUI();
            _dialogueUI?.Display(dialogue, dialogueElement, this);
            
            // 대화 인덱스 및 ID 처리
            if (HasLinkedDialogueID(dialogueElement, out _linkedDialogueID)) // 현재 대사에 연결된 대화 ID 존재 여부
            {
                _currentDialogueID = _linkedDialogueID;
                _currentScriptIndex = 0;
            }
            else
            {
                _currentScriptIndex++;
                if (_currentScriptIndex >= dialogue.dialogueElements.Count) // 대사 인덱스 초과 시
                {
                    _currentScriptIndex = 0;
                    _currentDialogueID++;
                }
            }

            // 선택지 존재 여부 처리
            _hasChoice = HasChoice(dialogueElement, out _choiceID);
        }
    }

    // 대사 문자 치환
    public string ReplaceDialogueText(string dialogueText)
    {
        return dialogueText.Replace("@", ","); // @ -> ,로 변경 ( 추후 플레이어 이름도 변경할 예정 ) 
    }

    private void EndDialogue()
    {
        _currentDialogues = null;
        
        // 플레이어 상태 전환
        if (SceneManager.GetActiveScene().buildIndex == 1)  
        {
            // 빌드 인덱스 1번 == Room 씬이라면 플레이어 상태 Room으로 전환 
            GameManager.Instance.playerState = GameManager.PlayerState.Room;
        }
        else
        {
            // 아니라면 None으로 전환 
            GameManager.Instance.playerState = GameManager.PlayerState.None;
        }

        switch (_dialogueType)
        {
            case DialogueType.RoomDialogue:
                break;
            
            case DialogueType.RoomNarration:
                break;
            
            case DialogueType.VerandaDialogue:
                // 일러스트 숨기기
                illustrationController.HideIllustration(_dialogueType);
                // 초상화 숨기기
                portraitController.HidePortrait(_dialogueType);
                // 표시 아이템 숨기기
                displayItemController.HideDisplayItem();
                break;
            
            case DialogueType.VerandaNarration:
                break;
            
            default:
                break;
        }

        // 대화 창 숨기기
        _dialogueUI?.Hide(this);
        
        Debug.Log("대화 종료.");
    }
    
    // 대화 UI 종류 선택
    private IDialogueUI SelectDialogueUI()
    {
        // 대화 화면 UI 처리
        IDialogueUI dialogueUI = null;
        switch (_dialogueType) 
        {
            case DialogueType.RoomDialogue:
                // 일반적인 방 대사창 UI 출력 
                dialogueUI = new RoomDialogueUI();
                break;
            
            case DialogueType.RoomNarration:
                // 방 대사창 나레이션 UI 출력 
                dialogueUI = new RoomNarrationUI();
                break;
            
            case DialogueType.VerandaDialogue:
                // 일반적인 베란다 대사창 UI 출력 
                dialogueUI = new VerandaDialogueUI();
                break;
            
            case DialogueType.VerandaNarration:
                // 베란다 대사창 나레이션 UI 출력 
                dialogueUI = new VerandaNarrationUI();
                break;
            
            default:
                Debug.LogError("씬 종류를 식별할 수 없습니다.");
                break;
        }
        return dialogueUI;
    }
    
    // 현재 대사에 선택지가 존재하는지 검사
    private bool HasChoice(DialogueElement dialogueElement, out int choiceID)
    {
        bool hasChoice = dialogueElement.choiceID != 0;
        choiceID = hasChoice ? dialogueElement.choiceID : 0;
        
        return hasChoice;
    }

    // 현재 대사에 연결된 대화 ID가 존재하는지 검사
    private bool HasLinkedDialogueID(DialogueElement dialogueElement, out int linkedDialogueID)
    {
        bool hasLinkedDialogueID = dialogueElement.linkedDialogueID != 0;
        linkedDialogueID = hasLinkedDialogueID ? dialogueElement.linkedDialogueID : 0;
        
        return hasLinkedDialogueID;
    }

    // 현재 대사에 일러스트가 존재하는지 검사
    private bool HasIllustration(DialogueElement dialogueElement)
    {
        return dialogueElement.illustrationName != "";
    }

    // 현재 대화에 초상화가 존재하는지 검사
    private bool HasCharacterPortrait(DialogueElement dialogueElement)
    {
        return dialogueElement.characterPortraitName != "";
    }
    
    // 현재 대사에 표시 아이템이 존재하는지 검사
    private bool HasDisplayItem(DialogueElement dialogueElement)
    {
        return dialogueElement.displayItemName != "";
    }
    
    private bool HasBGM(DialogueElement dialogueElement)
    {
        return dialogueElement.bgmName != "";
    }

    private bool HasSFX(DialogueElement dialogueElement)
    {
        return dialogueElement.sfxName != "";
    }
}
    