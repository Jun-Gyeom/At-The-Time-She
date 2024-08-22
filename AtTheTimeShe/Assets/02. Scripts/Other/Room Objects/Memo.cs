using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo : RoomObject
{
    public int bedEndingStateLinkedDialogueID;   // 침대 엔딩 상태일 때 출력될 대화 ID
    public override void Interact()
    {
        // 침대 엔딩 바로 전날인지 확인
        if (GameManager.Instance.BedEndingCount == GameManager.Instance.bedEnding.bedEndingCountForEndingCondition - 1)
        {
            DialogueManager.Instance.StartDialogue(bedEndingStateLinkedDialogueID);
        }

        // 이외
        else
        {
            // 대사 출력 
            DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
        }
    }
}
