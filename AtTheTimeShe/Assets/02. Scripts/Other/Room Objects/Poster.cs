using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poster : RoomObject
{
    public override void Interact()
    {
        // 대사 출력 
        DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
    }
}
