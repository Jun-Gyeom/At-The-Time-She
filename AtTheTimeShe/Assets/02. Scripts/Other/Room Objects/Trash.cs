using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : RoomObject
{
    public override void Interact()
    {
        Debug.Log("쓰레기가 있다.");
    }
}
