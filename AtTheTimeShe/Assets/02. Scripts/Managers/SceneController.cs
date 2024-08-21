using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum SceneName
{
    Title,
    Room,
    Dialogue
}
public class SceneController : Singleton<SceneController>
{
    public bool IsSceneChanging { get; private set; }            // 씬 전환 중인지 여부 
    public event Action OnFadeComplate;               // 씬 전환 페이드아웃 종료시 실행되는 이벤트  
    
    [Header("Settings")]
    public float fadeInOutDuration;             // 페이드인 페이드아웃에 걸리는 시간 
    public float fadingDuration;                // 페이드인 된 채로 대기하는 시간 
    public float dateTextInAnimationDuration;   // 날짜 텍스트 In 애니메이션에 걸리는 시간 
    public float dateTextOutAnimationDuration;  // 날짜 텍스트 Out 애니메이션에 걸리는 시간 
    
    [Header("Object")]
    public Image fadeImage;                     // 페이드인 페이드아웃에 사용할 이미지
    public GameObject fadeImageGameObject;
    public TMP_Text dataText;                   // 날짜 텍스트 
    public GameObject dataTextGameObject;   
    
    private WaitForSeconds _waitForFadeInOut;
    private WaitForSeconds _waitForFading;
    
    private void Start()
    {
        _waitForFadeInOut = new WaitForSeconds(fadeInOutDuration);
        _waitForFading = new WaitForSeconds(fadingDuration);
    }

    public void ChangeScene(SceneName sceneName)
    {
        if (IsSceneChanging) return;

        StartCoroutine(HandleSceneChange(sceneName));
    }

    // 씬 전환 코루틴 
    private IEnumerator HandleSceneChange(SceneName sceneName)
    {
        IsSceneChanging = true;

        // 페이드인
        FadeIn();
        yield return _waitForFadeInOut;

        // 씬 비동기 로딩 
        yield return StartCoroutine(LoadSceneAsync(sceneName));
        
        // Room 씬이라면 날짜 텍스트 띄우기 
        if (sceneName == SceneName.Room)
        {
            // 조금 대기
            yield return new WaitForSeconds(0.75f);
            
            ShowDateText();
            yield return new WaitForSeconds(dateTextInAnimationDuration);
        }
        
        // 대기
        yield return _waitForFading;

        // 띄웠던 날짜 텍스트 숨기기
        if (sceneName == SceneName.Room)
        {
            HideDateText();
            yield return new WaitForSeconds(dateTextOutAnimationDuration);
        }

        // 페이드아웃
        FadeOut();
        yield return _waitForFadeInOut;
        
        // 콜백 이벤트 실행 
        OnFadeComplate?.Invoke();
        OnFadeComplate = null;
        
        IsSceneChanging = false;
    }
    

    private IEnumerator LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneName);
        if (asyncOperation == null)
        {
            Debug.LogError($"존재하지 않는 씬 번호입니다. : {(int)sceneName}");
        }
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

    public void FadeIn()
    {
        // 알파 값을 1으로 서서히 변경
        fadeImageGameObject.SetActive(true);
        fadeImage.DOFade(1f, fadeInOutDuration); 
    }

    public void FadeOut()
    {
        // 알파 값을 0로 서서히 변경
        fadeImage.DOFade(0f, fadeInOutDuration).OnComplete(() =>
        {
            fadeImageGameObject.SetActive(false);
        });  
    }

    // 날짜 텍스트 보여주기 
    private void ShowDateText()
    {
        // DOText 애니메이션 재생 
        dataText.text = ""; 
        dataTextGameObject.SetActive(true);
        dataText.DOText(string.Format("Day {0}", GameManager.Instance.Date), dateTextInAnimationDuration);
    }

    // 날짜 텍스트 숨기기 
    private void HideDateText()
    {
        // 페이드아웃 애니메이션 재생
        // 날짜 트위닝 애니메이션 추가하기. -------------------( 일단 페이드아웃 )
        dataText.DOFade(0f, fadeInOutDuration).OnComplete(() =>
        {
            dataTextGameObject.SetActive(false);
            dataText.color = Color.white;
        });
    }
}
