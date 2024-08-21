using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ending : ScriptableObject
{
    public string endingName;                   // 엔딩 이름
    public string endingEnglishName;            // 엔딩 영문 이름 
    public bool isUnlocked;                     // 해당 엔딩을 봤는지 여부  

    public abstract void Play();

    public virtual void PlayTrailerDialogue()
    {
        
    }
}
