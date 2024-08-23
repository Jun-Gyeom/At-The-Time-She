using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ending : ScriptableObject
{
    public string endingType;                   // 어떤 엔딩인지 
    public string endingName;                   // 엔딩 이름
    public string endingEnglishName;            // 엔딩 영문 이름 
    public bool isUnlocked;                     // 해당 엔딩을 봤는지 여부  

    // 엔딩을 재생하는 추상 메서드 
    public abstract void Play();

    // 엔딩의 조건을 검사하는 추상 메서드 
    public abstract bool CheckEndingCondition();

    // 6일차 엔딩 암시 대화를 출력하는 메서드 
    public virtual void PlayTrailerDialogue()
    {
        
    }
}
