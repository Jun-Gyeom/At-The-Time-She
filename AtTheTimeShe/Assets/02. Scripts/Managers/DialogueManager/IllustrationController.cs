using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationController : Singleton<IllustrationController>
{
    private Dictionary<string, Sprite> _illustrations;    // 삽화 딕셔너리

    // 베란다 대화 GUI - 일러스트
    [HideInInspector] public GameObject verandaIllustrationGameObject;        // 일러스트 오브젝트
    [HideInInspector] public Image verandaIllustrationImage;                  // 일러스트 이미지

    private void Start()
    {
        _illustrations = ResourceManager.Instance.LoadAll<Sprite>("Illustration");
    }

    public void ShowIllustration(string illustrationName)
    {
        Debug.Log($"삽화 출력! >> {illustrationName}");
        Sprite illustration = _illustrations[illustrationName];
        if (!illustration)
        {
            Debug.LogError($"다음 이름의 삽화를 찾을 수 없습니다. >> {illustrationName}");
        }
        verandaIllustrationImage.sprite = illustration;
        verandaIllustrationGameObject.SetActive(true);
    }

    public void HideIllustration()
    {
        Debug.Log("삽화 숨기기!");
        verandaIllustrationGameObject.SetActive(false);
    }
}
