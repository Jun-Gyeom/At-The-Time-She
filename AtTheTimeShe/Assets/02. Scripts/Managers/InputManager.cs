using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public static GameControls InputAsset;
    
    // 델리게이트 선언
    public delegate void KeyAction();
    
    // 입력 이벤트 선언
    public static event KeyAction OnInteract;       // Left Button
    public static event KeyAction OnNextDialogue;   // Space, Left Button
    public static event KeyAction OnChooseChoice;   // Enter
    public static event KeyAction OnOpenOptionMenu; // Escape

    private new void Awake()
    {
        base.Awake();

        InputAsset = new GameControls();
    }
    private void Update()
    {
        InputAsset.Room.Interact.performed += ctx => Interact();
        InputAsset.Dialogue.NextDialogue.performed += ctx => NextDialogue();
        InputAsset.Dialogue.ChooseChoice.performed += ctx => ChooseChoice();
        InputAsset.Dialogue.Pause.performed += ctx => OpenOptionMenu();
    }

    private void OnEnable()
    {
        InputAsset.Enable();
    }

    private void OnDisable()
    {
        InputAsset.Disable();
    }

    private void Interact()
    {
        OnInteract?.Invoke();
    }

    private void NextDialogue()
    {
        OnNextDialogue?.Invoke();
    }
    
    private void ChooseChoice()
    {
        OnChooseChoice?.Invoke();
    }
    
    private void OpenOptionMenu()
    {
        OnOpenOptionMenu?.Invoke();
    }
}
