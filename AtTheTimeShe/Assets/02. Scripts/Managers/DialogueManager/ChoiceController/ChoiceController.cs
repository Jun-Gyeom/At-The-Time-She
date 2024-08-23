using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceController : Singleton<ChoiceController>
{
    [SerializeField] private List<Choice> _choices;        // 선택지 리스트
    
    // 베란다 대화 GUI - 선택지
    [HideInInspector] public GameObject verandaChoicePanelGameObject;         // 선택지 패널 오브젝트
    [HideInInspector] public List<Button> verandaChoiceButtons;               // 선택지 버튼 리스트
    
    // 방 텍스트 박스 GUI - 선택지
    [HideInInspector] public GameObject roomChoicePanelGameObject;            // 방 선택지 패널 게임 오브젝트
    [HideInInspector] public List<Button> roomChoiceButtons;                  // 방 선택지 버튼 리스트 

    public bool IsDisplayedChoice
    {
        get { return _isDisplayedChoice; }
    }
    
    private bool _isDisplayedChoice;    // 선택지가 출력되어 있는지 여부
    
    private new void Awake()
    {
        base.Awake();
        
        CSVParser csvParser = new CSVParser();
        
        // 선택지 데이터 파싱
        TextAsset choicesCSVFile = Resources.Load<TextAsset>("CSV/Choices");    // 경로 입력하기.
        _choices = csvParser.LoadDialogueChoices(choicesCSVFile);
    }

    // 대화의 선택지를 출력
    public void ShowChoice(int choiceID, DialogueType dialogueType, DialogueManager dialogueManager)
    {
        _isDisplayedChoice = true;
        Choice choice = _choices.Find(choice => choice.choiceID == choiceID);
        if (choice.Equals(null))
        {
            Debug.LogError($"다음 ID의 선택지를 찾을 수 없습니다. >> {choiceID}");
        }

        switch (dialogueType)
        {
            // 방 대화창 선택지 띄우기 
            case DialogueType.RoomDialogue:
            case DialogueType.RoomNarration:
                SetChoice(roomChoicePanelGameObject, roomChoiceButtons, choice, dialogueType, dialogueManager);
                break;
            
            // 베란다 대화창 선택지 띄우기 
            case DialogueType.VerandaDialogue:
            case DialogueType.VerandaNarration:
                SetChoice(verandaChoicePanelGameObject, verandaChoiceButtons, choice, dialogueType, dialogueManager);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없어 선택지를 출력할 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }


    }

    private void HideChoice(DialogueType dialogueType)
    {
        switch (dialogueType)
        {
            // 방 대화창 선택지 닫기 
            case DialogueType.RoomDialogue:
            case DialogueType.RoomNarration:
                roomChoicePanelGameObject.SetActive(false);
                break;
            
            // 베란다 대화창 선택지 닫기
            case DialogueType.VerandaDialogue:
            case DialogueType.VerandaNarration:
                verandaChoicePanelGameObject.SetActive(false);
                break;
            
            default:
                Debug.LogError($"씬 타입을 식별할 수 없어 출력된 선택지 패널을 닫을 수 없습니다. 씬 타입 : {dialogueType}");
                break;
        }
    }

    private void SetChoice(GameObject choicePanel, List<Button> buttons, Choice choice, DialogueType dialogueType, DialogueManager dialogueManager)
    {
        for (int i = 0; i < choice.choiceElements.Count; i++)
        {
            int index = i;  // Lambda 클로저 문제를 해결하기 위해 지역 변수에 값을 복사. 
            
            buttons[index].GetComponentInChildren<TMP_Text>().text =
                dialogueManager.ReplaceDialogueText(choice.choiceElements[index].choiceText);
            int linkedDialogueID = choice.choiceElements[index].linkedDialogueID;
            buttons[index].onClick.RemoveAllListeners();
            buttons[index].onClick.AddListener(() => _isDisplayedChoice = false);
            
            // 연결된 대화 ID가 0이라면 선택 시, 대화 종료 
            if (linkedDialogueID == 0)
            {
                buttons[index].onClick.AddListener(() => { DialogueManager.Instance.EndDialogue(); });

            }
            else
            {
                buttons[index].onClick.AddListener(() => dialogueManager.NextDialogue(linkedDialogueID));
            }
            
            buttons[index].onClick.AddListener(() => HideChoice(dialogueType));
            
            // 선택지 선택 시 실행할 메서드 등록
            buttons[index].onClick.AddListener(() => choice.choiceElements[index].triggerEvent.Invoke());
            Debug.Log(choice.choiceElements[index].triggerEvent);
            
            // 선택지 선택 조건이 불충족이라면 Disable 
            bool condition = choice.choiceElements[index].condition.Invoke();
            if (!condition)
            {
                buttons[index].GetComponentInChildren<TMP_Text>().color = new Color(0.15f, 0.15f, 0.15f);
                buttons[index].interactable = false;
            }
            else
            {
                buttons[index].GetComponentInChildren<TMP_Text>().color = new Color(1f, 1f, 1f);
                buttons[index].interactable = true;
            }
        }
        choicePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }
}
