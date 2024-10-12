using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VerandaDialogueUI : IDialogueUI
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
        dialogueManager.verandaDialoguePanelGameObject.SetActive(true);    // 대화 패널 활성화
        dialogueManager.verandaTalkerText.text = $"[ {dialogueManager.ReplaceDialogueText(dialogue.talker)} ]";                   // 화자 텍스트 적용
        dialogueManager.verandaDialogueText.text = "";  // 텍스트 초기화 
        
        if (!dialogueManager.isNotInitDialogue)
        {
            // 화면 출력 대기  
            IsFading = true;
            CanvasGroup dialogueGroup = dialogueManager.verandaDialoguePanelGameObject.GetComponent<CanvasGroup>();
            dialogueGroup.alpha = 0f;
            dialogueGroup.DOFade(1f, 1f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    IsFading = false;
                    // 대화 텍스트 DOTween 타이핑 시작 
                    DialogueManager.Instance.StartTypingEffect(dialogueManager.verandaDialogueText,
                        dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));
                });
            dialogueManager.isNotInitDialogue = true;
        }
        else
        {
            // 대화 텍스트 DOTween 타이핑 시작 
            DialogueManager.Instance.StartTypingEffect(dialogueManager.verandaDialogueText,
                dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));
        }
    }

    public void Hide(DialogueManager dialogueManager, bool doFade)
    {
        if (doFade)
        {
            CanvasGroup dialogueGroup = dialogueManager.verandaDialoguePanelGameObject.GetComponent<CanvasGroup>();
            dialogueGroup.DOFade(0f, 1f).OnComplete(() =>
            {
                dialogueManager.verandaDialoguePanelGameObject.SetActive(false);    // 대화 패널 비활성화
            });
        }
        else
        {
            dialogueManager.verandaDialoguePanelGameObject.SetActive(false);    // 대화 패널 비활성화
        }
        
    }
}
