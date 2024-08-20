using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : RoomObject
{
    public int goodStateLinkedDialogueID;   // 냉장고 상태가 좋을 때 출력될 대화 ID

    [Header("Setting")] 
    public int goodChoiceNumberForCleanRefrigerator;    // 깨끗한 냉장고 상태를 위한 좋은 선택 갯수 
    public override void Interact()
    {
        // 냉장고 상태가 좋을 때 
        if (GameManager.Instance.GoodChoiceNumber >= goodChoiceNumberForCleanRefrigerator)
        {
            DialogueManager.Instance.StartDialogue(goodStateLinkedDialogueID);
        }

        // 상태가 좋지 않을 때
        else
        {
            DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
        }
    }
}
