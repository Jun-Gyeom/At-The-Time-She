using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueUI
{
    public bool IsFading { get; set; }
    public void Display(Dialogue dialogue, DialogueElement dialogueElement, DialogueManager dialogueManager);
    public void Hide(DialogueManager dialogueManager, bool doFade);
}