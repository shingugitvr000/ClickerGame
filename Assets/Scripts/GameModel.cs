// GameModel.cs - ���� ���¸� �����ϴ� ��
using System;
using System.Numerics;
using UnityEngine;

public class GameModel
{
    // ���� ���� ����
    private BigInteger _score;
    private BigInteger _clickValue;

    // �̺�Ʈ - ������ ���� �� ��𵨿� �˸�
    public event Action OnScoreChanged;
    public event Action OnClickValueChanged;

    // �Ӽ�
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

    // ������
    public GameModel()
    {
        Score = BigInteger.Zero;
        ClickValue = BigInteger.One;
    }

    // ���� ���� �޼���
    public void Click()
    {
        Score = BigNumberManager.Instance.Add(Score, ClickValue);
    }

    // GameModel.cs�� �߰��� �κ�
    public void MultiClick(int times)
    {
        try
        {
            // times�� Ŭ�� ȿ�� ����
            BigInteger addAmount = BigNumberManager.Instance.Multiply(ClickValue, new BigInteger(times));
            Score = BigNumberManager.Instance.Add(Score, addAmount);
        }
        catch (Exception e)
        {
            Debug.LogError($"MultiClick ����: {e.Message}");
            // ���� �߻� �� �� �� ������ ������� ����
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

    // ����/�ε� �޼��� (������ ����)
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