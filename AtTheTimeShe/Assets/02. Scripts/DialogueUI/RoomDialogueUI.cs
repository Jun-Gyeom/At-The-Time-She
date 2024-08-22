using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDialogueUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        // 방 텍스트 박스 텍스트 대사 적용 및 활성화 
        dialogueManager.roomTalkerText.text = $"[ {dialogueManager.ReplaceDialogueText(dialogue.talker)} ]";             // 화자 텍스트 적용 
        
        // 대화 텍스트 DOTween 타이핑 시작 
        DialogueManager.Instance.StartTypingEffect(dialogueManager.roomTextBoxText, dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));
        
        dialogueManager.roomTextBoxPanelGameObject.SetActive(true);
    }
    
    public void Hide(DialogueManager dialogueManager)
    {
        dialogueManager.roomTextBoxPanelGameObject.SetActive(false);
    }
}
