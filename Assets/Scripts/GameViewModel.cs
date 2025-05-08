// GameViewModel.cs - �𵨰� �� ������ �Ű�ü
using System;
using System.Numerics;
using UnityEngine;

public class GameViewModel : MonoBehaviour
{
    private GameModel _model;

    // �信�� ������ �Ӽ�
    public string FormattedScore => BigNumberManager.Instance.FormatBigNumber(_model.Score);
    public string FormattedClickValue => BigNumberManager.Instance.FormatBigNumber(_model.ClickValue);

    // �並 ���� �̺�Ʈ
    public event Action OnUIUpdateRequired;

    void Awake()
    {
        // �� �ʱ�ȭ
        _model = new GameModel();

        // �� �̺�Ʈ ����
        _model.OnScoreChanged += () => OnUIUpdateRequired?.Invoke();
        _model.OnClickValueChanged += () => OnUIUpdateRequired?.Invoke();

        // ����� ���� �ҷ�����
        _model.LoadGame();
    }

    // �信�� ȣ���� �޼���
    public void HandleClick()
    {
        _model.Click();
    }

    public bool TryUpgradeClick()
    {
        return _model.TryUpgradeClickValue(10); // 10�� ����
    }

    // GameViewModel.cs�� �߰��� �κ�
    public void HandleMultiClick(int times)
    {
        _model.MultiClick(times);
    }

    public string GetMultiClickButtonCost()
    {
        // 100�� Ŭ�� ��ư�� ��� - ���� Ŭ�� ��ġ�� 80��� ����
        BigInteger cost = _model.ClickValue * 80;
        return BigNumberManager.Instance.FormatBigNumber(cost);
    }
      
    public string GetUpgradeCost()
    {
        return BigNumberManager.Instance.FormatBigNumber(_model.ClickValue * 10);
    }

    // �������� �ڵ� ����
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