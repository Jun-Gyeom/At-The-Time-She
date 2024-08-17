using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veranda : RoomObject
{
    public override void Interact()
    {
        GameManager.Instance.DidTodayDialogue = true;
        
        SceneController.Instance.ChangeScene(Scene.Dialogue);
    }
}
