using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : RoomObject
{
    public override void Interact()
    {
        // 대사 출력 
        DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
    }
}
