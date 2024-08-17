using System;
using System.Collections.Generic;
using UnityEngine;

public static class CSVParser
{
    // 대화 씬 데이터 파싱
    public static List<DialogueScene> LoadDialogues(TextAsset csvFile)
    {
        List<DialogueScene> dialogueScenes = new List<DialogueScene>();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        DialogueScene dialogueScene = null;
        Dialogue dialogue = null;

        for (int i = 2; i < lines.Length; i++)
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
                    sceneType = SceneType.TryParse(values[1].Trim(), out SceneType sceneType) ? sceneType : SceneType.None,
                    dialogues = new List<Dialogue>()
                };
            }

            if (!string.IsNullOrWhiteSpace(values[2])) // 대화 ID가 존재하는 경우 새 대화 생성
            {
                if (dialogue != null && dialogueScene != null) // 이전 대화 추가
                {
                    dialogueScene.dialogues.Add(dialogue);
                }
                dialogue = new Dialogue
                {
                    dialogueID = int.TryParse(values[2].Trim(), out int dialogueID) ? dialogueID : 0,
                    characterPortrait = values[3].Trim(),
                    talker = values[4].Trim(),
                    dialogueElements = new List<DialogueElement>()
                };
            }

            // 대화 요소 추가
            if (dialogue != null)
            {
                DialogueElement dialogueElement = new DialogueElement
                {
                    dialogueText = values[5].Trim(),
                    choiceID = int.TryParse(values[6], out int choiceID) ? choiceID : 0,
                    linkedDialogueID = int.TryParse(values[7], out int linkedDialogueID) ? linkedDialogueID : 0,
                    sfxName = values[8].Trim(),
                    bgmName = values[9].Trim(),
                    displayItemName = values[10].Trim(),
                    illustrationName = values[11].Trim()
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
    public static List<Choice> LoadDialogueChoices(TextAsset csvFile)
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
                // 이 예제에서는 condition과 triggerEvent가 단순화된 형태로 제공됩니다. 
                // 실제 구현에서는 런타임에 평가되어야 할 복잡한 로직을 포함할 수 있습니다.
                condition = () => true,  // 조건 로직 필요 시 여기에 구현
                triggerEvent = () => { }, // 이벤트 로직 필요 시 여기에 구현
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
}
