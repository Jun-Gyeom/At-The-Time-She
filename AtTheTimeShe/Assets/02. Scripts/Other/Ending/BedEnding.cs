using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ending/BedEnding")]
public class BedEnding : Ending
{
    public int endingDialogueSceneID;               // 엔딩 대화 씬 ID
    public int bedEndingCountForEndingCondition;    // 침대 엔딩 조건 충족을 위해 필요한 침대 엔딩 카운트 

    public override void Play()
    {
        Debug.Log("침대엔딩 플레이!");
        
        // 침대 엔딩 플레이
        DialogueManager.Instance.StartDialogue(endingDialogueSceneID);
    }

    public override bool CheckEndingCondition()
    {
        return GameManager.Instance.BedEndingCount >= bedEndingCountForEndingCondition;
    }
}
