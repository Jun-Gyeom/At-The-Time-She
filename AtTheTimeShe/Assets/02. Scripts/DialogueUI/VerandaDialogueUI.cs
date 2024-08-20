using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerandaDialogueUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        dialogueManager.verandaTalkerText.text = $"[ {dialogueManager.ReplaceDialogueText(dialogue.talker)} ]";                   // 화자 텍스트 적용 
        dialogueManager.verandaDialogueText.text = dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText);    // 대화 텍스트 적용
        dialogueManager.verandaDialoguePanelGameObject.SetActive(true);    // 대화 패널 활성화
    }

    public void Hide(DialogueManager dialogueManager)
    {
        dialogueManager.verandaDialoguePanelGameObject.SetActive(false);    // 대화 패널 비활성화
    }
}
