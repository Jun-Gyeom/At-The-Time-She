using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public enum PlayerState { None, Room, Dialogue }
    
    public PlayerState playerState = PlayerState.None;  // 플레이어 상태
    public int Date { get; set; }                       // 게임 날짜
    public bool DidTodayDialogue { get; set; }          // 오늘 대화를 진행하였는지 여부 
    public int GoodChoiceNumber { get; set; }           // 좋은 선택 횟수 
    public int BadChoiceNumber { get; set; }            // 나쁜 선택 횟수

    [Header("Settings")]
    public List<Date> dates;                            // 날짜 리스트 
    
    private Camera _mainCamera;
    private GameObject _currentOnPointObject = null;
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneController.Instance.ChangeScene(Scene.Title);
        }
        
        
        // 마우스 포인터 오브젝트 감지 
        CheckOnPointObject();

        if (playerState == PlayerState.None)            // None 상태
        {
            InputManager.InputAsset.Room.Disable();
            InputManager.InputAsset.Dialogue.Disable();
        }
        else if (playerState == PlayerState.Room)       // 방 상태 
        {
            InputManager.InputAsset.Room.Enable();
            InputManager.InputAsset.Dialogue.Disable();
        }
        else if (playerState == PlayerState.Dialogue)   // 대화 상태 
        {
            InputManager.InputAsset.Room.Disable();
            InputManager.InputAsset.Dialogue.Enable();
        }
    }

    // 마우스 포인터가 콜라이더를 가진 오브젝트를 감지
    private void CheckOnPointObject()
    {
        // 씬 전환 중이라면 감지하지 않음 
        if (SceneController.Instance.IsSceneChanging)
        {
            _currentOnPointObject = null;
            return;
        }
        
        // 마우스 위치를 가져오기
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // 마우스 위치를 월드 좌표로 변환
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        
        // 해당 위치에서 2D 레이캐스트 수행
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        // 레이캐스트가 콜라이더와 충돌했는지 확인
        if (hit.collider != null)
        {
            _currentOnPointObject = hit.collider.gameObject;
        }
        else
        {
            _currentOnPointObject = null;
        }
    }

    // 다음 날로 
    public void ToNextDay()
    {
        // 날짜 1 증가 
        Date += 1;
        
        // 오늘 대화 여부 초기화
        DidTodayDialogue = false;
    }
    
    private void OnEnable()
    {
        InputManager.OnNextDialogue += OnNextDialogue;
        InputManager.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        InputManager.OnNextDialogue -= OnNextDialogue;
        InputManager.OnInteract -= OnInteract;
    }

    private void OnInteract()
    {
        if (_currentOnPointObject != null)
        {
            bool isRoomObject = _currentOnPointObject.TryGetComponent(out RoomObject roomObject);
            if (isRoomObject)
            {
                roomObject.Interact();
            }
        }
    }
    
    public void OnNextDialogue()
    {
        Debug.Log("다음 대화!");
        DialogueManager.Instance.NextDialogue();
    }
}
