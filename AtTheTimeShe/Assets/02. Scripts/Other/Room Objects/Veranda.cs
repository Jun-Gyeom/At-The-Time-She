using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veranda : RoomObject
{
    public override void Interact()
    {
        DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
        
        /*
        GameManager.Instance.DidTodayDialogue = true;
        
        SceneController.Instance.ChangeScene(SceneName.Dialogue);
        */
    }
}
