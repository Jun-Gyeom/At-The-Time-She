using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class DialogueScene
{
    public int sceneID;                 // 대화 씬 ID
    public List<Dialogue> dialogues;    // 대화 리스트
}
