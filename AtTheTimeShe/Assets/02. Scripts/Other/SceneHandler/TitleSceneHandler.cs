using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneHandler : MonoBehaviour, ISceneInitializer
{
    private void Start()
    {
        InitializeScene();
    }
    
    public void InitializeScene()
    {
        // 플레이어 상태 None으로 변경. 
        GameManager.Instance.playerState = GameManager.PlayerState.None;
        
        // 타이틀 배경음악 재생
        AudioManager.Instance.PlayBGM("Title_BGM");
        
        // 게임 데이터 초기화 
        GameManager.Instance.InitializeGameData();
    }

    // 게임 시작 버튼 이벤트에 등록 
    public void StartGame()
    {
        SceneController.Instance.ChangeScene(SceneName.Room);
    }

    // 설정 버튼 이벤트에 등록 
    public void ShowSettings()
    {
        
    }

    // 설정 끄기 버튼 이벤트에 등록 
    public void HideSettings()
    {
        
    }

    // 게임 종료 버튼 이벤트에 등록 
    public void QuitGame()
    {
        StartCoroutine(HandleQuit());
    }

    private IEnumerator HandleQuit()
    {
        // 페이드인
        SceneController.Instance.FadeIn();
        yield return new WaitForSeconds(SceneController.Instance.fadeInOutDuration);
        
        // 게임 종료
        Application.Quit();
    }
}

