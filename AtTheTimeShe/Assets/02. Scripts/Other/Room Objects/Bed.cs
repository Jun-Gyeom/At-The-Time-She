using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : RoomObject
{
    public override void Interact()
    {
        ToNextDay();
    }

    private void ToNextDay()
    {
        GameManager.Instance.ToNextDay();
        
        // Room 씬으로 전환 
        SceneController.Instance.ChangeScene(Scene.Room);
    }
}
