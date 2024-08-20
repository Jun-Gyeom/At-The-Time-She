using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ending")]
public class Ending : ScriptableObject
{
    public string endingName;           // 엔딩 이름
    public int endingDialogueSceneID;   // 엔딩 대화 씬 ID
    public bool isUnlocked;             // 해당 엔딩을 봤는지 여부  
}
