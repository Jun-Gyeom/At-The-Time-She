using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TitleSceneHandler : MonoBehaviour, ISceneInitializer
{
    public Animator creditsAnimator;        // 크레딧 애니메이터 
    public GameObject creditsPanel;         // 크레딧 패널 게임 오브젝트 
    public TMP_Text creditText1;
    public TMP_Text creditText2;
    public TMP_Text creditText3;
    
    private bool _isCreditsAnimating = false;
    
    private Vector2 initialPosText1;
    private Vector2 initialPosText2;
    private Vector2 initialPosText3;
    private void Start()
    {
        InitializeScene();
        
        // 초기 위치 저장
        initialPosText1 = creditText1.GetComponent<RectTransform>().anchoredPosition;
        initialPosText2 = creditText2.GetComponent<RectTransform>().anchoredPosition;
        initialPosText3 = creditText3.GetComponent<RectTransform>().anchoredPosition;

        // 씬 전환 중인지 여부
        if (SceneController.Instance.IsSceneChanging)
        {
            // 페이드아웃 후 타이틀 배경음악 재생.
            SceneController.Instance.OnFadeComplate += () => AudioManager.Instance.PlayBGM("Title_BGM");
        }
        else
        {
            // 타이틀 배경음악 재생
            AudioManager.Instance.PlayBGM("Title_BGM");
        }
    }
    
    public void InitializeScene()
    {
        // 플레이어 상태 None으로 변경. 
        GameManager.Instance.playerState = GameManager.PlayerState.None;
        
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
        creditsPanel.SetActive(true);
    }
    
    public void ShowCredits()
    {
        if (_isCreditsAnimating) return; // 애니메이션 중이면 리턴

        _isCreditsAnimating = true;
        creditsPanel.SetActive(true);

        // 애니메이션 시작 전 초기 위치로 재설정
        creditText1.GetComponent<RectTransform>().anchoredPosition = initialPosText1;
        creditText2.GetComponent<RectTransform>().anchoredPosition = initialPosText2;
        creditText3.GetComponent<RectTransform>().anchoredPosition = initialPosText3;

        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(creditText1.GetComponent<RectTransform>().DOAnchorPosX(75f, 1f))
            .Append(creditText2.GetComponent<RectTransform>().DOAnchorPosX(75f, 1f))
            .Append(creditText3.GetComponent<RectTransform>().DOAnchorPosX(75f, 1f))
            .OnComplete(() => _isCreditsAnimating = false); // 애니메이션 완료 후 상태 해제
    }

    public void HideCredits()
    {
        if (_isCreditsAnimating) return; // 애니메이션 중이면 리턴

        _isCreditsAnimating = true;

        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(creditText1.DOFade(0f, 1f))
            .Join(creditText2.DOFade(0f, 1f))
            .Join(creditText3.DOFade(0f, 1f))
            .OnComplete(() =>
            {
                creditsPanel.SetActive(false);
                ResetCreditTextAlpha();
                _isCreditsAnimating = false; // 애니메이션 완료 후 상태 해제
            });
    }
    
    private void ResetCreditTextAlpha()
    {
        // 텍스트가 다시 보이도록 알파 값을 초기화
        creditText1.color = new Color(1f, 1f, 1f, 1f);
        creditText2.color = new Color(1f, 1f, 1f, 1f);
        creditText3.color = new Color(1f, 1f, 1f, 1f);
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

