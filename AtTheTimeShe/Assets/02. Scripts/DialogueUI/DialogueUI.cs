using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : IDialogueUI
{
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        dialogueManager.verandaDialogueText.text = dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText);    // 대화 텍스트 적용
        dialogueManager.verandaCharacterPortraitImage.sprite = ResourceManager.Instance.Load<Sprite>(dialogue.characterPortrait);  // 캐릭터 초상화 적용
        dialogueManager.verandaDialoguePanelGameObject.SetActive(true);    // 대화 패널 활성화
    }

    public void Hide(DialogueManager dialogueManager)
    {
        Debug.Log("대화 UI 숨기기!");
        dialogueManager.verandaDialoguePanelGameObject.SetActive(false);    // 대화 패널 비활성화
    }
}
