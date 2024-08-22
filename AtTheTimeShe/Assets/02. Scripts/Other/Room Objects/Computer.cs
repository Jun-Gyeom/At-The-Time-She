using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Computer : RoomObject
{
    public int canBuyGiftStateLinkedDialogueID;                 // 선물 구매가 가능할 때 출력될 대화 ID
    public int afterDialogueStateLinkedDialogueID;              // 대화 이후일 때 출력될 대화 ID 
    public int hasGiftStateLinkedDialogueID;                    // 선물 소지한 상태일 때 출력될 대화 ID
    public int completeTodayWorkStateLinkedDialogueID;          // 오늘 일을 완료한 상태일 때 출력될 대화 ID            
    public int beforeDialogueAndDayZeroStateLinkedDialogueID;   // 0일차 대화 이전일 때 출력 될 대화 ID
    public int bedEndingStateLinkedDialogueID;                  // 침대 엔딩 상태일 때 출력될 대화 ID
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
            // 대화 이후인지 확인
            if (GameManager.Instance.DidTodayDialogue)
            {
                // 컴퓨터 사용 불가능 
                
                DialogueManager.Instance.StartDialogue(afterDialogueStateLinkedDialogueID);
            }

            // 대화 이전일 때
            else
            {
                // 0일차인지 확인
                if (GameManager.Instance.Date == 0)
                {
                    // 베란다부터... 대화 출력 
                    DialogueManager.Instance.StartDialogue(beforeDialogueAndDayZeroStateLinkedDialogueID);
                }

                // 그 외
                else
                {
                    // 일한 횟수가 선물 구매에 충분한지 확인 
                    if (GameManager.Instance.WorkNumber >= GameManager.Instance.workNumberForHasGift)
                    {
                        // 이미 선물을 구매하였는지 확인
                        if (GameManager.Instance.HasGift)
                        {
                            // 선물 구매 불가 
                        
                            DialogueManager.Instance.StartDialogue(hasGiftStateLinkedDialogueID);
                        }

                        // 선물 구매하지 않았을 떄
                        else
                        {
                            // 선물 구매 대화 출력 
                            DialogueManager.Instance.StartDialogue(canBuyGiftStateLinkedDialogueID);
                        }
                    }
                    else
                    {
                        // 오늘 이미 일을 했는지 확인
                        if (GameManager.Instance.DidTodayWork)
                        {
                            // 일 불가
                        
                            DialogueManager.Instance.StartDialogue(completeTodayWorkStateLinkedDialogueID);
                        }

                        // 오늘 일을 하지 않았을 때
                        else
                        {
                            // 일하시겠습니까? 대화 출력 
                            DialogueManager.Instance.StartDialogue(linkedDialogueSceneID);
                        }
                    }
                }
            }
        }
    }
}
