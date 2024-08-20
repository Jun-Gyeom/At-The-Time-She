using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class RoomObject : MonoBehaviour, IInteractable
{
    public Collider2D interactRange;    // 상호작용 범위
    public int linkedDialogueSceneID;   // 연결된 대화 씬 ID 

    public abstract void Interact();
}
