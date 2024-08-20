using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerandaNarrationUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        dialogueManager.verandaNarrationText.text = dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText);    // 나레이션 텍스트 적용
        dialogueManager.verandaNarrationPanelGameObject.SetActive(true);    // 나레이션 패널 활성화
    }

    public void Hide(DialogueManager dialogueManager)
    {
        dialogueManager.verandaNarrationPanelGameObject.SetActive(false);    // 나레이션 패널 비활성화
    }
}
