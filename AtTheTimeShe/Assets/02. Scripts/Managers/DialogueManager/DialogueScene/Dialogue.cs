using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType
{
    None,
    RoomDialogue,       // 일반적인 방 대사창 UI
    RoomNarration,      // 방 대사창 나레이션 UI
    VerandaDialogue,    // 일반적인 베란다 대사창 UI
    VerandaNarration    // 베란다 대사창 나레이션 UI 
};

[Serializable]
public class Dialogue
{   
    public int dialogueID;                          // 대화 ID
    public DialogueType dialogueType;               // 대화 종류 
    public string talker;                           // 화자
    public List<DialogueElement> dialogueElements;  // 대화 요소 리스트
}
