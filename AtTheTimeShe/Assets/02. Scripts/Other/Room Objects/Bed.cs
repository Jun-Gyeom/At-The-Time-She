using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : RoomObject
{
    public int goodStateLinkedDialogueID;   // 침대 상태가 좋을 때 출력될 대화 ID

    [Header("Setting")] 
    public int goodChoiceNumberForNeatBed;    // 정돈된 침대 상태를 위한 좋은 선택 갯수 
    public override void Interact()
    {
        ToNextDay();
    }

    private void ToNextDay()
    {
        // 침대 상태가 좋을 때 
        if (GameManager.Instance.GoodChoiceNumber >= goodChoiceNumberForNeatBed)
        {
            DialogueManager.Instance.StartDialogue(goodStateLinkedDialogueID);
        }

        // 상태가 좋지 않을 때
        else
        {
            DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
        }
        
        /*
        GameManager.Instance.ToNextDay();
        
        // Room 씬으로 전환 
        SceneController.Instance.ChangeScene(SceneName.Room);
        */
    }
}
