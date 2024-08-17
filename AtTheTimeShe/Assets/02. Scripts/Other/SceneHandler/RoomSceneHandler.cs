using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSceneHandler : MonoBehaviour, ISceneInitializer
{
    [Header("Setting")] 
    public int goodChoiceNumberForNeatBedding;      // 정돈된 이불을 위한 좋은 선택 횟수
    public int goodChoiceNumberForCleanTrash;       // 쓰레기 청소를 위한 좋은 선택 횟수
    
    [Header("Reference")]
    public GameObject neatBeddingGameObject;        // 정돈된 이불 게임 오브젝트
    public GameObject noneNeatBeddingGameObject;    // 정돈되지 않은 이불 게임 오브젝트
    public GameObject trashGameObject;              // 쓰레기 봉지 게임 오브젝트
    public GameObject trashColliderGameObject;      // 쓰레기 봉지 콜라이더 게임 오브젝트 
    public GameObject darkPanelGameObject;          // 어두운 패널 게임 오브젝트 
    
    private void Start()
    {
        InitializeScene();
    }

    public void InitializeScene()
    {
        // 플레이어 상태 Room으로 변경. 
        GameManager.Instance.playerState = GameManager.PlayerState.Room;

        UpdateRoomState();
        
        // 오늘 대화 여부에 따라 방 밝기 적용 
        darkPanelGameObject.SetActive(GameManager.Instance.DidTodayDialogue);
    }

    // 방 상태 업데이트 메서드 
    public void UpdateRoomState()
    {
        Debug.Log("방 정리!");
        int goodChoiceNumber = GameManager.Instance.GoodChoiceNumber;
        
        // 이불 정돈
        if (goodChoiceNumber >= goodChoiceNumberForNeatBedding)
        {
            noneNeatBeddingGameObject.SetActive(false);
            neatBeddingGameObject.SetActive(true);
        }
        
        // 쓰레기 청소 
        bool trashActive = !(goodChoiceNumber >= goodChoiceNumberForCleanTrash);
        trashGameObject.SetActive(trashActive);
        trashColliderGameObject.SetActive(trashActive);
    }
}
