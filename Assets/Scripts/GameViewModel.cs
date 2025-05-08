// GameViewModel.cs - 모델과 뷰 사이의 매개체
using System;
using System.Numerics;
using UnityEngine;

public class GameViewModel : MonoBehaviour
{
    private GameModel _model;

    // 뷰에서 접근할 속성
    public string FormattedScore => BigNumberManager.Instance.FormatBigNumber(_model.Score);
    public string FormattedClickValue => BigNumberManager.Instance.FormatBigNumber(_model.ClickValue);

    // 뷰를 위한 이벤트
    public event Action OnUIUpdateRequired;

    void Awake()
    {
        // 모델 초기화
        _model = new GameModel();

        // 모델 이벤트 구독
        _model.OnScoreChanged += () => OnUIUpdateRequired?.Invoke();
        _model.OnClickValueChanged += () => OnUIUpdateRequired?.Invoke();

        // 저장된 게임 불러오기
        _model.LoadGame();
    }

    // 뷰에서 호출할 메서드
    public void HandleClick()
    {
        _model.Click();
    }

    public bool TryUpgradeClick()
    {
        return _model.TryUpgradeClickValue(10); // 10배 증가
    }

    // GameViewModel.cs에 추가할 부분
    public void HandleMultiClick(int times)
    {
        _model.MultiClick(times);
    }

    public string GetMultiClickButtonCost()
    {
        // 100번 클릭 버튼의 비용 - 현재 클릭 가치의 80배로 설정
        BigInteger cost = _model.ClickValue * 80;
        return BigNumberManager.Instance.FormatBigNumber(cost);
    }
      
    public string GetUpgradeCost()
    {
        return BigNumberManager.Instance.FormatBigNumber(_model.ClickValue * 10);
    }

    // 정기적인 자동 저장
    private float _saveTimer;
    private const float SaveInterval = 60f;

    void Update()
    {
        _saveTimer += Time.deltaTime;
        if (_saveTimer >= SaveInterval)
        {
            _model.SaveGame();
            _saveTimer = 0f;
        }
    }

    void OnApplicationQuit()
    {
        _model.SaveGame();
    }
}