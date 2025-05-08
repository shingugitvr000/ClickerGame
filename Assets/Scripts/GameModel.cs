// GameModel.cs - 게임 상태를 관리하는 모델
using System;
using System.Numerics;
using UnityEngine;

public class GameModel
{
    // 게임 상태 변수
    private BigInteger _score;
    private BigInteger _clickValue;

    // 이벤트 - 데이터 변경 시 뷰모델에 알림
    public event Action OnScoreChanged;
    public event Action OnClickValueChanged;

    // 속성
    public BigInteger Score
    {
        get => _score;
        private set
        {
            _score = value;
            OnScoreChanged?.Invoke();
        }
    }

    public BigInteger ClickValue
    {
        get => _clickValue;
        private set
        {
            _clickValue = value;
            OnClickValueChanged?.Invoke();
        }
    }

    // 생성자
    public GameModel()
    {
        Score = BigInteger.Zero;
        ClickValue = BigInteger.One;
    }

    // 게임 로직 메서드
    public void Click()
    {
        Score = BigNumberManager.Instance.Add(Score, ClickValue);
    }

    // GameModel.cs에 추가할 부분
    public void MultiClick(int times)
    {
        try
        {
            // times번 클릭 효과 적용
            BigInteger addAmount = BigNumberManager.Instance.Multiply(ClickValue, new BigInteger(times));
            Score = BigNumberManager.Instance.Add(Score, addAmount);
        }
        catch (Exception e)
        {
            Debug.LogError($"MultiClick 오류: {e.Message}");
            // 오류 발생 시 좀 더 안전한 방법으로 증가
            for (int i = 0; i < times; i++)
            {
                Score = BigNumberManager.Instance.Add(Score, ClickValue);
            }
        }
    }

    public bool TryUpgradeClickValue(BigInteger multiplier)
    {
        BigInteger cost = BigNumberManager.Instance.Multiply(ClickValue, multiplier);

        if (BigNumberManager.Instance.IsGreaterOrEqual(Score, cost))
        {
            Score = BigNumberManager.Instance.Add(Score, -cost);
            ClickValue = BigNumberManager.Instance.Multiply(ClickValue, multiplier);
            return true;
        }

        return false;
    }

    // 저장/로드 메서드 (간단한 구현)
    public void SaveGame()
    {
        PlayerPrefs.SetString("Score", Score.ToString());
        PlayerPrefs.SetString("ClickValue", ClickValue.ToString());
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            try
            {
                Score = BigInteger.Parse(PlayerPrefs.GetString("Score", "0"));
                ClickValue = BigInteger.Parse(PlayerPrefs.GetString("ClickValue", "1"));
            }
            catch (Exception)
            {
                Score = BigInteger.Zero;
                ClickValue = BigInteger.One;
            }
        }
    }
}