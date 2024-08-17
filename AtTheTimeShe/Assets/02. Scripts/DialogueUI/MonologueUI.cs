using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        dialogueManager.verandaMonologueText.text = dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText);  // 독백 텍스트 적용
        dialogueManager.verandaMonologuePanelGameObject.SetActive(true);    // 독백 패널 활성화
    }
    
    public void Hide(DialogueManager dialogueManager)
    {
        Debug.Log("독백 UI 숨기기!");
        dialogueManager.verandaMonologuePanelGameObject.SetActive(false);    // 독백 패널 비활성화
    }
}
