using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{   
    public int dialogueID;                          // 대화 ID
    public string characterPortrait;                // 캐릭터 초상화
    public string talker;                           // 화자
    public List<DialogueElement> dialogueElements;  // 대화 요소 리스트
}
