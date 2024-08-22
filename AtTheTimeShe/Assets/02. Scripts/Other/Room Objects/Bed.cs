using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : RoomObject
{
    public int goodStateLinkedDialogueID;           // 침대 상태가 좋을 때 출력될 대화 ID
    public int beforeDialogueStateLinkedDialogueID; // 대화 이전일 때 출력될 대화 ID 

    [Header("Setting")] 
    public int goodChoiceNumberForNeatBed;    // 정돈된 침대 상태를 위한 좋은 선택 갯수 
    public override void Interact()
    {
        // 대화 이후인지 확인 
        if (GameManager.Instance.DidTodayDialogue)
        {
            // 침대 사용 가능 

            UseTheBed();
        }

        // 대화 이전일 때 
        else
        {
            // 1일차 또는 2일차인지 또는 침대 엔딩 조건 카운트가 1 이상인지 확인
            if (GameManager.Instance.Date == 1 || 
                GameManager.Instance.Date == 2 || 
                GameManager.Instance.BedEndingCount > 0)
            {
                // 예외적으로 침대 사용 가능 
                
                UseTheBed();
            }

            // 일반적으로 침대 사용 불가능 
            else
            {
                DialogueManager.Instance.StartDialogue(beforeDialogueStateLinkedDialogueID);
            }
        }
    }

    private void UseTheBed()
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
    }
}
