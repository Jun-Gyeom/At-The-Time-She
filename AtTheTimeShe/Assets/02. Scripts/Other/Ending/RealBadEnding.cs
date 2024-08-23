using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(menuName = "SO/Ending/RealBadEnding")]
public class RealBadEnding : Ending
{
    public string consoleMessage;
    
    public override void Play()
    {
        Debug.Log("진짜 배드 엔딩 플레이!");
    
        ProcessStartInfo processInfo = new ProcessStartInfo();
    
        // 현재 운영체제가 Windows인지 확인 
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // CMD 실행 및 명령어 전달 
            processInfo.FileName = "cmd.exe";
            processInfo.Arguments = $"/c echo {consoleMessage} & pause";  // 사용자 입력 대기를 위해 'pause' 추가
        }
    
        // 현재 운영체제가 MacOS 또는 Linux인지 확인 
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            // Bash 실행 및 명령어 전달 
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"echo {consoleMessage}; read -p 'Press any key to continue...'\"";  // 사용자 입력 대기를 위해 'read' 추가
        }
    
        // 그 외 지원하는 않는 운영체제인지 확인 
        else
        {
            Debug.LogWarning("This platform is not supported.");
            return;
        }
    
        // 프로그램이 백그라운드에서 실행되지 않도록 설정 
        processInfo.CreateNoWindow = false;
    
        // 셸을 통해 프로그램 실행 
        processInfo.UseShellExecute = true;

        // 프로그램 실행 
        Process.Start(processInfo);
    
        // 게임을 종료 
        Application.Quit();
    }

    public override bool CheckEndingCondition()
    {
        return GameManager.Instance.BadChoiceNumber > GameManager.Instance.GoodChoiceNumber &&
               GameManager.Instance.GoodChoiceNumber == 0;
    }
}
