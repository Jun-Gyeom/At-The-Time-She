using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVParser
{
    // 대화 씬 데이터 파싱
    public List<DialogueScene> LoadDialogues(TextAsset csvFile)
    {
        List<DialogueScene> dialogueScenes = new List<DialogueScene>();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        DialogueScene dialogueScene = null;
        Dialogue dialogue = null;

        for (int i = 2; i < lines.Length; i++) // 첫 번째 줄과 두 번째 줄은 헤더이므로 건너뜁니다.
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            string[] values = lines[i].Split(',');

            if (!string.IsNullOrWhiteSpace(values[0])) // 씬 ID가 존재하는 경우 새 씬 생성
            {
                if (dialogueScene != null) // 이전 씬에 대한 처리
                {
                    if (dialogue != null) // 마지막 대화 추가
                    {
                        dialogueScene.dialogues.Add(dialogue);
                        dialogue = null; // 마지막 대화 초기화
                    }
                    dialogueScenes.Add(dialogueScene); // 씬을 리스트에 추가
                }
                dialogueScene = new DialogueScene
                {
                    sceneID = int.TryParse(values[0].Trim(), out int sceneID) ? sceneID : 0,
                    dialogues = new List<Dialogue>()
                };
            }

            if (!string.IsNullOrWhiteSpace(values[1])) // 대화 ID가 존재하는 경우 새 대화 생성
            {
                if (dialogue != null && dialogueScene != null) // 이전 대화 추가
                {
                    dialogueScene.dialogues.Add(dialogue);
                }
                dialogue = new Dialogue
                {
                    dialogueID = int.TryParse(values[1].Trim(), out int dialogueID) ? dialogueID : 0,
                    dialogueType = DialogueType.TryParse(values[2].Trim(), out DialogueType dialogueType) ? dialogueType : DialogueType.None,
                    talker = values[3].Trim(),
                    dialogueElements = new List<DialogueElement>()
                };
            }

            // 대화 요소 추가
            if (dialogue != null)
            {
                DialogueElement dialogueElement = new DialogueElement
                {
                    dialogueText = values[4].Trim(),
                    choiceID = int.TryParse(values[5], out int choiceID) ? choiceID : 0,
                    linkedDialogueID = int.TryParse(values[6], out int linkedDialogueID) ? linkedDialogueID : 0,
                    illustrationName = values[7].Trim(),
                    characterPortraitName = values[8].Trim(),
                    displayItemName = values[9].Trim(),
                    sfxName = values[10].Trim(),
                    bgmName = values[11].Trim(),
                };
                dialogue.dialogueElements.Add(dialogueElement);
            }
        }

        // 파일의 끝 처리: 마지막 대화 및 씬 추가
        if (dialogue != null && dialogueScene != null)
        {
            dialogueScene.dialogues.Add(dialogue);
        }
        if (dialogueScene != null)
        {
            dialogueScenes.Add(dialogueScene);
        }

        return dialogueScenes;
    }
    
    // 대화 선택지 데이터 파싱
    public List<Choice> LoadDialogueChoices(TextAsset csvFile)
    {
        List<Choice> dialogueChoices = new List<Choice>();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        Choice currentChoice = null;

        for (int i = 2; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            string[] values = lines[i].Split(',');

            // 새로운 선택지 ID가 시작되는 경우
            if (!string.IsNullOrWhiteSpace(values[0]))
            {
                if (currentChoice != null)
                {
                    dialogueChoices.Add(currentChoice);
                }
                currentChoice = new Choice
                {
                    choiceID = int.TryParse(values[0].Trim(), out int choiceID) ? choiceID : 0,
                    choiceElements = new List<ChoiceElement>()
                };
            }

            // 선택지 요소 추가
            ChoiceElement choiceElement = new ChoiceElement
            {
                choiceText = values[1].Trim(),
                condition = TextToCondition(values[2]),  // 선택지 선택 조건 델리게이트에 반환값 bool 형태의 메서드 추가  
                triggerEvent = TextToTriggerEvent(values[3]), // 선택지 선택 시 발동 이벤트 델리게이트에 메서드 추가 
                linkedDialogueID = int.TryParse(values[4].Trim(), out int linkedDialogueID) ? linkedDialogueID : 0
            };
            currentChoice.choiceElements.Add(choiceElement);
        }

        // 마지막 선택지 추가
        if (currentChoice != null)
        {
            dialogueChoices.Add(currentChoice);
        }

        return dialogueChoices;
    }

    // 텍스트를 선택지 선택 조건 Func<bool>로 변환시켜주는 메서드 
    private Func<bool> TextToCondition(string text)
    {
        Func<bool> condition;
        
        switch (text)
        {
            // 해당 란이 비어있다면 조건이 없는 것으로 간주 
            case "":
                condition = () => true;
                break;
            
            // 선물을 소지하고 있는지 여부 
            case "HasGift":
                condition = () => GameManager.Instance.HasGift;
                break;
            
            default:
                Debug.LogError($"다음 이름의 메서드를 찾을 수 없습니다 : {text}");
                condition = () => true;
                break;
        }

        return condition;
    }

    // 텍스트를 발생 이벤트 Action으로 변환시켜주는 메서드 
    private Action TextToTriggerEvent(string text)
    {
        Action triggerEvent;

        switch (text)
        {
            // 해당 란이 비어있다면 이벤트가 없는 것으로 간주 
            case "":
                triggerEvent = () => { };
                break;
            
            // 대화
            // 대화 - 좋은 선택지 선택 횟수 증가 
            case "GoodChoice":
                triggerEvent = () => { GameManager.Instance.GoodChoiceNumber++; };
                break;
            
            // 대화 - 나쁜 선택지 선택 횟수 증가 
            case "BadChoice":
                triggerEvent = () => { GameManager.Instance.BadChoiceNumber++; };
                break;
            
            // 대화 - 선물 주기
            case "Gift":
                triggerEvent = () => { GameManager.Instance.GiveGift = true; };
                break;
            
            // 방 - 대화 창 닫기 
            
            // 방 - 다음 날로 ( 침대 )
            
            // 방 - 베란다로 ( 베란다 )
            
            // 방 - 일하기 ( 컴퓨터 )
            case "Work":
                triggerEvent = () => { GameManager.Instance.Work(); };
                break;
            
            // 타이틀 - 세이브파일 불러오기 
            // 추후 구현. 
            
            default:
                Debug.LogError($"다음 이름의 메서드를 찾을 수 없습니다 : {text}");
                triggerEvent = () => { };
                break;
        }

        return triggerEvent;
    }
}
