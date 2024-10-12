using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNarrationUI : IDialogueUI
{
    private bool _isFading;
    public bool IsFading
    {
        get
        {
            return _isFading;
        }
        set
        {
            _isFading = value;
        }
    }
    
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager)
    {
        // 방 나레이션 텍스트 DOTween 타이핑 시작 
        DialogueManager.Instance.StartTypingEffect(dialogueManager.roomNarrationText, dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));

        dialogueManager.roomNarrationTextPanelGameObject.SetActive(true);               // 방 나레이션 패널 활성화 
    }

    public void Hide(DialogueManager dialogueManager, bool doFade)
    {
        dialogueManager.roomNarrationTextPanelGameObject.SetActive(false);               // 방 나레이션 패널 비활성화 
    }
}
