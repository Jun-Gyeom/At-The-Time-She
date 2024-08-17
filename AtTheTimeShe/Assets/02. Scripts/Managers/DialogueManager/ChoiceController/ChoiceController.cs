using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceController : Singleton<ChoiceController>
{
    private List<Choice> _choices;        // 선택지 리스트
    
    // 베란다 대화 GUI - 선택지
    [HideInInspector] public GameObject verandaChoicePanelGameObject;         // 선택지 패널 오브젝트
    [HideInInspector] public List<Button> verandaChoiceButtons;               // 선택지 버튼 리스트

    public bool IsDisplayedChoice
    {
        get { return _isDisplayedChoice; }
    }
    
    private bool _isDisplayedChoice;    // 선택지가 출력되어 있는지 여부
    
    private void Start()
    {
        // 선택지 데이터 파싱
        TextAsset choicesCSVFile = Resources.Load<TextAsset>("CSV/Choices");    // 경로 입력하기.
        _choices = CSVParser.LoadDialogueChoices(choicesCSVFile);
    }

    // 대화의 선택지를 출력
    public void ShowChoice(int choiceID, DialogueManager dialogueManager)
    {
        _isDisplayedChoice = true;
        Choice choice = _choices.Find(choice => choice.choiceID == choiceID);
        if (choice.Equals(null))
        {
            Debug.LogError($"다음 ID의 선택지를 찾을 수 없습니다. >> {choiceID}");
        }

        for (int i = 0; i < choice.choiceElements.Count; i++)
        {
            verandaChoiceButtons[i].GetComponentInChildren<TMP_Text>().text
                = dialogueManager.ReplaceDialogueText(choice.choiceElements[i].choiceText);
            int linkedDialogueID = choice.choiceElements[i].linkedDialogueID;
            verandaChoiceButtons[i].onClick.RemoveAllListeners();
            verandaChoiceButtons[i].onClick.AddListener(() => _isDisplayedChoice = false);
            verandaChoiceButtons[i].onClick.AddListener(() => dialogueManager.NextDialogue(linkedDialogueID));
            verandaChoiceButtons[i].onClick.AddListener(() => HideChoice());
            verandaChoicePanelGameObject.SetActive(true);
        }
    }

    private void HideChoice()
    {
        verandaChoicePanelGameObject.SetActive(false);
    }
}
