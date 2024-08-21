using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : RoomObject
{
    public int canBuyGiftStateLinkedDialogueID;     // 선물 구매가 가능할 때 출력될 대화 ID
    public override void Interact()
    {
        // 일한 횟수가 선물 구매에 충분한지 확인 
        if (GameManager.Instance.WorkNumber >= GameManager.Instance.workNumberForHasGift)
        {
            // 일한 횟수가 선물을 구매하기에 충분할 때 선물 구매 대화 출력 
            DialogueManager.Instance.StartDialogue(canBuyGiftStateLinkedDialogueID);
        }
        else
        {
            // 일하시겠습니까? 대화 출력 
            DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
        }
    }
}
