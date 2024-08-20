using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Date")]
public class Date : ScriptableObject
{
    public int date;                        // 날짜 
    public int currentDateDialogueSceneID;  // 현재 날짜 대화 씬 ID
}
