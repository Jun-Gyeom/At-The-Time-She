using System;
using System.Collections.Generic;

public enum SceneType
{
    None,
    General,
    Ending,
    True_Ending
};

[Serializable]
public class DialogueScene
{
    public int sceneID;                 // 대화 씬 ID
    public SceneType sceneType;         // 대화 씬 종류
    public List<Dialogue> dialogues;    // 대화 리스트
}
