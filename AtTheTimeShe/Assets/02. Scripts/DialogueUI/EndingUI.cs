using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        Debug.Log($"엔딩 UI 출력! >> {dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText)}");
    }
    
    public void Hide(DialogueManager dialogueManager)
    {
        Debug.Log("엔딩 UI 숨기기!");
    }
}
