using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerandaNarrationUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        // 나레이션 텍스트 DOTween 타이핑 시작 
        DialogueManager.Instance.StartTypingEffect(dialogueManager.verandaNarrationText, dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));

        dialogueManager.verandaNarrationPanelGameObject.SetActive(true);    // 나레이션 패널 활성화
    }

    public void Hide(DialogueManager dialogueManager)
    {
        dialogueManager.verandaNarrationPanelGameObject.SetActive(false);    // 나레이션 패널 비활성화
    }
}
