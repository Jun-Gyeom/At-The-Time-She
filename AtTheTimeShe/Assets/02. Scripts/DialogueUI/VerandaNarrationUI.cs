using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VerandaNarrationUI : IDialogueUI
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
        dialogueManager.verandaNarrationPanelGameObject.SetActive(true);    // 나레이션 패널 활성화
        dialogueManager.verandaNarrationText.text = "";  // 텍스트 초기화 

        if (!dialogueManager.isNotInitDialogue)
        {
            // 화면 출력 대기  
            IsFading = true;
            CanvasGroup narrationGroup = dialogueManager.verandaNarrationPanelGameObject.GetComponent<CanvasGroup>();
            narrationGroup.alpha = 0f;
            narrationGroup.DOFade(1f, 1f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    IsFading = false;
                    // 나레이션 텍스트 DOTween 타이핑 시작 
                    DialogueManager.Instance.StartTypingEffect(dialogueManager.verandaNarrationText,
                        dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));
                });
            dialogueManager.isNotInitDialogue = true;
        }
        else
        {
            // 나레이션 텍스트 DOTween 타이핑 시작 
            DialogueManager.Instance.StartTypingEffect(dialogueManager.verandaNarrationText,
                dialogueManager.ReplaceDialogueText(dialogueElement.dialogueText));
        }
    }

    public void Hide(DialogueManager dialogueManager, bool doFade)
    {
        if (doFade)
        {
            // 페이드 아웃
            CanvasGroup narrationGroup = dialogueManager.verandaNarrationPanelGameObject.GetComponent<CanvasGroup>();
            narrationGroup.DOFade(0f, 1f).OnComplete(() =>
            {
                dialogueManager.verandaNarrationPanelGameObject.SetActive(false); // 나레이션 패널 비활성화
            });
        }
        else
        {
            dialogueManager.verandaNarrationPanelGameObject.SetActive(false); // 나레이션 패널 비활성화
        }
    }
}
