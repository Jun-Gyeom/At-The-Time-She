using System;
using UnityEngine.Serialization;

[Serializable]
public class DialogueElement
{
    public string dialogueText;         // 대사
    public int choiceID;                // 선택지 ID
    public int linkedDialogueID;        // 연결된 대사 ID
    public string illustrationName;     // 삽화 이름
    public string displayItemName;      // 표시 아이템 이름
    public string characterPortraitName;// 초상화 이름 
    public string sfxName;              // 효과음 이름
    public string bgmName;              // 배경음악 이름
}
