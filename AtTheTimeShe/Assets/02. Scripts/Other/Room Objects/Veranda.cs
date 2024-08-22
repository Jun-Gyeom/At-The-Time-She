using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Veranda : RoomObject
{
    public int afterDialogueStateLinkedDialogueID;          // 대화 이후일 때 출력될 대화 ID 
    public int bedEndingCountHasStateLinkedDialogueID;      // 침대 엔딩 카운트가 1 이상일 때 출력될 대화 ID 
    public int bedEndingStateLinkedDialogueID;              // 침대 엔딩 바로 전 날 상태일 때 출력될 대화 ID
    
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
            // 침대 엔딩 카운트가 1 이상인지 확인
            if (GameManager.Instance.BedEndingCount >= 1)
            {
                DialogueManager.Instance.StartDialogue(bedEndingCountHasStateLinkedDialogueID);
            }

            // 그 외
            else
            {
                // 대화 이후인지 확인
                if (GameManager.Instance.DidTodayDialogue)
                {
                    // 베란다 사용 불가능 
            
                    DialogueManager.Instance.StartDialogue(afterDialogueStateLinkedDialogueID);
                }

                // 대화 이전일 때 
                else
                {
                    // 베란다 사용 가능
            
                    DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
                }
            }
        }
    }
}
