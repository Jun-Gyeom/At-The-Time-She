using System;

[Serializable]
public class ChoiceElement
{
    public string choiceText;       // 선택지 대사
    public Func<bool> condition;    // 선택 조건
    public Action triggerEvent;     // 발동 이벤트
    public int linkedDialogueID;    // 연결된 대화 ID
}
