using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class Choice
{
    public int choiceID;                        // 선택지 ID
    public List<ChoiceElement> choiceElements;  // 선택지 요소 리스트
}
