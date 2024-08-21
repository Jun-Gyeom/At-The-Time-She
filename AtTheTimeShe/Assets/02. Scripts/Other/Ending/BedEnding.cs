using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ending/BedEnding")]
public class BedEnding : Ending
{
    public int endingDialogueSceneID;           // 엔딩 대화 씬 ID

    public override void Play()
    {
        Debug.Log("침대엔딩 플레이!");
    }
}
