// BigNumberManager.cs - ū ���ڸ� �����ϴ� �� Ŭ����
using System;
using System.Numerics;
using UnityEngine;

public class BigNumberManager
{
    // �̱��� ���� (�ʿ信 ���� ���)
    private static BigNumberManager _instance;
    public static BigNumberManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BigNumberManager();
            return _instance;
        }
    }

    // ���� ǥ�� ���� ����
    private readonly string[] standardSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    // ������ �޼��� - ū ���ڸ� �б� ���� �������� ��ȯ
    public string FormatBigNumber(BigInteger number)
    {
        if (number == BigInteger.Zero)
            return "0";

        // �⺻ ���̻� (K, M, B, T ��)
        string[] standardSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

        try
        {
            // 10^36 �̸��� ���� ó��
            if (BigInteger.Log10(number) < 36)
            {
                int suffixIndex = 0;
                BigInteger tempNum = number;

                while (tempNum >= 1000 && suffixIndex < standardSuffixes.Length - 1)
                {
                    tempNum /= 1000;
                    suffixIndex++;
                }

                // BigInteger�� double�� ��ȯ�Ͽ� �Ҽ��� ó��
                double displayValue;

                if (tempNum <= 1000000) // �����ϰ� double�� ��ȯ ������ ����
                {
                    displayValue = (double)tempNum;
                }
                else
                {
                    // �ʹ� ū ���ڴ� ������ �� �ڸ��� ���
                    BigInteger divisor = BigInteger.Pow(10, (int)BigInteger.Log10(tempNum) - 2);
                    displayValue = (double)(tempNum / divisor) / 10.0;
                }

                // �Ҽ��� ���� ����
                if (displayValue >= 100)
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
                else if (displayValue >= 10)
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
                else
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
            }
            else // 10^36 �̻��� ���ĺ� ���� ���
            {
                double log10 = (double)BigInteger.Log10(number);
                int alphabetOrder = (int)(log10 - 33) / 3;

                char firstChar = (char)('a' + (alphabetOrder / 26));
                char secondChar = (char)('a' + (alphabetOrder % 26));
                string suffix = new string(new[] { firstChar, secondChar });

                // �Ҽ��� ���
                double mantissa = Math.Pow(10, log10 - Math.Floor(log10 / 3) * 3);

                // �Ҽ��� ���� ���� (�׻� 2�ڸ� �Ҽ��� ǥ��)
                return $"{mantissa:F2}{suffix}";
            }
        }
        catch (Exception)
        {
            // ���� ó��: �ſ� ū ����
            double log10 = (double)BigInteger.Log10(number);

            // ���ĺ� ���� ���
            int alphabetOrder = (int)(log10 - 33) / 3;
            if (alphabetOrder >= 0)
            {
                char firstChar = (char)('a' + Math.Min(alphabetOrder / 26, 25));
                char secondChar = (char)('a' + Math.Min(alphabetOrder % 26, 25));
                string suffix = new string(new[] { firstChar, secondChar });

                // 3�ڸ����� �� ���̻�, �� �ȿ��� 1.00~999.99 ���� ���
                double mantissa = Math.Pow(10, log10 - Math.Floor(log10 / 3) * 3);

                // �Ҽ��� ǥ�� Ȯ��
                return $"{mantissa:F2}{suffix}";
            }
            else
            {
                int suffixIndex = Math.Min((int)(log10 / 3), standardSuffixes.Length - 1);
                return $"{Math.Pow(10, log10 - suffixIndex * 3):F2}{standardSuffixes[suffixIndex]}";
            }
        }
    }

    // ���� ���� - �����÷ο� ���� �� ������ BigInteger ����
    public BigInteger Add(BigInteger a, BigInteger b)
    {
        return a + b;
    }

    // ���� ���� - �����÷ο� ���� �� ������ BigInteger ����
    public BigInteger Multiply(BigInteger a, BigInteger b)
    {
        return a * b;
    }

    // �� ���� - �� BigInteger ��
    public bool IsGreaterOrEqual(BigInteger a, BigInteger b)
    {
        return a >= b;
    }
}