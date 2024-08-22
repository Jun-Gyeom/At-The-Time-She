using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public enum PlayerState { None, Room, Dialogue }
    
    public PlayerState playerState = PlayerState.None;  // 플레이어 상태
    public int Date { get; set; }                       // 게임 날짜
    public bool DidTodayDialogue { get; set; }          // 오늘 대화를 진행하였는지 여부 
    public bool DidTodayWork { get; set; }              // 오늘 컴퓨터로 일을 하였는지 여부
    public int GoodChoiceNumber { get; set; }           // 좋은 선택 횟수 
    public int BadChoiceNumber { get; set; }            // 나쁜 선택 횟수
    public bool PresentGift { get; set; }               // 선물을 주었는지 여부 
    public bool HasGift { get; set; }                   // 선물을 소지하고 있는지 여부 
    public int WorkNumber { get; set; }                 // 컴퓨터로 일한 횟수
    public int BedEndingCount { get; set; }             // 침대 엔딩 조건 카운트 
    public SceneName CurrentSceneBuildIndex { get; private set; } // 현재 씬
    public Ending SelectedEnding { get; private set; }  // 결정된 게임 엔딩 

    [Header("Setting")]
    public List<Date> dates;                            // 날짜 리스트 
    public int workNumberForHasGift;                    // 선물을 얻기 위해 필요한 일한 횟수 
    public int checkEndingDay;                          // 엔딩 조건을 판별할 날짜 
    public int endingDay;                               // 엔딩을 플레이할 날짜 

    [Header("Ending")] 
    public TrueEnding trueEnding;                       // 진 엔딩
    public HappyEnding happyEnding;                     // 해피 엔딩
    public NormalEnding normalEnding;                   // 노멀 엔딩
    public BedEnding bedEnding;                         // 침대 엔딩
    public RealBadEnding realBadEnding;                 // 진짜 배드 엔딩
    
    private Camera _mainCamera;
    private GameObject _currentOnPointObject = null;
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
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

    // 다음 날로 ( 침대 사용하기 ) 
    public void ToNextDay()
    {
        // 날짜 1 증가 
        Date += 1;
        
        // 대화 이전에 침대를 사용했는지 확인 
        if (!DidTodayDialogue) 
        {
            // 침대 엔딩 조건 카운트 + 1
            BedEndingCount++;
            
            // 침대 엔딩 조건이 충족되었는지 확인
            if (bedEnding.CheckEndingCondition())
            {
                // 침대 엔딩 결정 
                SelectedEnding = bedEnding;
                
                // Ending 씬 로드
                SceneController.Instance.ChangeScene(SceneName.Ending);
                return;
            }
        }
        
        // 오늘 대화 여부 초기화
        DidTodayDialogue = false;

        // 오늘 일 여부 초기화 
        DidTodayWork = false;

        // 엔딩을 보여줄 날인지 확인 
        if (Date == endingDay)
        {
            // Ending 씬 로드
            SceneController.Instance.ChangeScene(SceneName.Ending);
        }
        else
        {
            // Room 씬 로드
            SceneController.Instance.ChangeScene(SceneName.Room);
        }
    }

    // 컴퓨터로 일하는 메서드 
    public void Work()
    {
        // 일한 횟수 증가 
        WorkNumber++;
        
        // 오늘 일 완료 여부 참으로 변경 
        DidTodayWork = true; 
        
        Debug.Log($"일을 완료했습니다! 일한 횟수 : {WorkNumber}");
    }

    public void BuyGift()
    {
        // 일한 횟수가 충족되었다면 선물 획득. 
        if (WorkNumber >= workNumberForHasGift)
        {
            HasGift = true;
            Debug.Log("선물을 구매했습니다! ");
        }
    }

    // 매우 예외적인 메서드이므로, 해당 케이스 이외엔 사용하지 말 것.
    // 현재 나쁜 선택지가 한번도 없었다면, 5일차 All Good 대화 ID로 이동하는 메서드 ( 개 똥 메서드 ) 
    public void CheckDay5AllGood()
    {
        // 좋은 선택지이므로 좋은 선택 횟수 증가 
        GoodChoiceNumber++;
        
        if (BadChoiceNumber == 0)
        {
            // ------------------------( 여기 파라미터를 5일차 All Good 대화 시작 지점 대화 ID 적으면 됨. ) 
            DialogueManager.Instance.NextDialogue(1 );
        }
    }
    
    // 게임 데이터 초기화 
    public void InitializeGameData()
    {
        // 플레이어 상태 초기화 
        playerState = PlayerState.None;
        
        // 게임 날짜 초기화 
        Date = 0;
        
        // 오늘 대화를 진행하였는지 여부 초기화 
        DidTodayDialogue = false;

        // 오늘 컴퓨터로 일을 하였는지 여부 초기화 
        DidTodayWork = false;

        // 좋은 선택 횟수 초기화 
        GoodChoiceNumber = 0;

        // 나쁜 선택 횟수 초기화 
        BadChoiceNumber = 0;

        // 선물을 주었는지 여부 초기화 
        PresentGift = false;

        // 선물을 소지하고 있는지 여부 초기화 
        HasGift = false;

        // 컴퓨터로 일한 횟수 초기화 
        WorkNumber = 0;

        // 침대 엔딩 조건 카운트 초기화 
        BedEndingCount = 0;

        // 결정된 게임 엔딩 초기화 
        SelectedEnding = null;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        InputManager.OnNextDialogue += OnNextDialogue;
        InputManager.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
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

    // 엔딩 조건 체크하여 유효한 엔딩을 반환하는 메서드 
    public Ending CheckEnding()
    {
        Ending ending;
        
        // 엔딩 체크하기
        // 진 엔딩 조건 체크 ( 옆집 누나 ) 
        if (trueEnding.CheckEndingCondition())
        {
            ending = trueEnding;
        }
        
        // 해피 엔딩 조건 체크 ( 친한 누나 )
        else if (happyEnding.CheckEndingCondition())
        {
            ending = happyEnding;
        }
        
        // 노멀 엔딩 조건 체크 ( 그저 그런.. )
        else if (normalEnding.CheckEndingCondition())
        {
            ending = normalEnding;
        }
        
        // 진짜 베드 엔딩 조건 체크 ( 손절이야 )
        else if (realBadEnding.CheckEndingCondition())
        {
            ending = realBadEnding;
        }

        // 예외 처리 
        else
        {
            Debug.Log("유효한 엔딩이 존재하지 않습니다.");
            ending = null;
        }

        return ending;
    }

    public void PlayEnding(Ending ending)
    {
        Debug.Log($"다음 엔딩이 재생됩니다. : {ending.endingName}");
        ending.Play();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 씬 정보 업데이트 
        CurrentSceneBuildIndex = (SceneName)scene.buildIndex;

        // 이미 결정된 엔딩이 없는지 확인
        if (SelectedEnding == null)
        {
            // 엔딩 조건 판정 날짜인지 확인 
            if (Date == checkEndingDay)
            {
                // 엔딩 조건 판정 
                SelectedEnding = CheckEnding();
            
                Debug.Log($"결정된 게임 엔딩 : {SelectedEnding}");
            }
        }
    }
}
