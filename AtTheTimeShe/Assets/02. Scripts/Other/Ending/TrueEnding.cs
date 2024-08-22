using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ending/TrueEnding")]
public class TrueEnding : Ending
{
    public int endingDialogueSceneID;           // 엔딩 대화 씬 ID
    public int endingTrailerDialogueSceneID;    // 6일차 엔딩 암시 대화 씬 ID
    
    public override void Play()
    {
        Debug.Log("진엔딩 플레이!");
        
        // 진 엔딩 플레이 
        DialogueManager.Instance.StartDialogue(endingDialogueSceneID);
    }

    public override bool CheckEndingCondition()
    {
        return GameManager.Instance.GoodChoiceNumber > GameManager.Instance.BadChoiceNumber &&
               GameManager.Instance.BadChoiceNumber == 0 &&
               GameManager.Instance.PresentGift;
    }

    public override void PlayTrailerDialogue()
    {
        DialogueManager.Instance.StartDialogue(endingTrailerDialogueSceneID);
    }
}
