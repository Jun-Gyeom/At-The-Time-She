using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNarrationUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        dialogueManager.roomNarrationText.text = dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText);     // 방 나레이션 텍스트 적용 
        dialogueManager.roomNarrationTextPanelGameObject.SetActive(true);               // 방 나레이션 패널 활성화 
    }

    public void Hide(DialogueManager dialogueManager)
    {
        dialogueManager.roomNarrationTextPanelGameObject.SetActive(false);               // 방 나레이션 패널 비활성화 
    }
}
