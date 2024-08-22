using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : RoomObject
{
    public int goodStateLinkedDialogueID;   // 냉장고 상태가 좋을 때 출력될 대화 ID
    public int bedEndingStateLinkedDialogueID;   // 침대 엔딩 상태일 때 출력될 대화 ID

    [Header("Setting")] 
    public int goodChoiceNumberForCleanRefrigerator;    // 깨끗한 냉장고 상태를 위한 좋은 선택 갯수 
    public override void Interact()
    {
        // 침대 엔딩 바로 전날인지 확인
        if (GameManager.Instance.BedEndingCount == GameManager.Instance.bedEnding.bedEndingCountForEndingCondition - 1)
        {
            DialogueManager.Instance.StartDialogue(bedEndingStateLinkedDialogueID);
        }

        else
        {
            // 냉장고 상태가 좋을 때 
            if (GameManager.Instance.IsGoodStateOfRefrigerator)
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

    private void Awake()
    {
        // 아침인지 확인
        if (!GameManager.Instance.DidTodayDialogue)
        {
            // 좋은 선택을 충분히 하였는지 확인  
            if (GameManager.Instance.GoodChoiceNumber >= goodChoiceNumberForCleanRefrigerator)
            {
                // 좋은 상태로 변경
                GameManager.Instance.IsGoodStateOfRefrigerator = true;
            }
        }
    }
}