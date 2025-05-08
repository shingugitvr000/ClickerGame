// BigNumberManager.cs - 큰 숫자만 관리하는 모델 클래스
using System;
using System.Numerics;
using UnityEngine;

public class BigNumberManager
{
    // 싱글톤 패턴 (필요에 따라 사용)
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

    // 숫자 표시 단위 정의
    private readonly string[] standardSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    // 포맷팅 메서드 - 큰 숫자를 읽기 쉬운 형식으로 변환
    public string FormatBigNumber(BigInteger number)
    {
        if (number == BigInteger.Zero)
            return "0";

        // 기본 접미사 (K, M, B, T 등)
        string[] standardSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

        try
        {
            // 10^36 미만의 숫자 처리
            if (BigInteger.Log10(number) < 36)
            {
                int suffixIndex = 0;
                BigInteger tempNum = number;

                while (tempNum >= 1000 && suffixIndex < standardSuffixes.Length - 1)
                {
                    tempNum /= 1000;
                    suffixIndex++;
                }

                // BigInteger를 double로 변환하여 소수점 처리
                double displayValue;

                if (tempNum <= 1000000) // 안전하게 double로 변환 가능한 범위
                {
                    displayValue = (double)tempNum;
                }
                else
                {
                    // 너무 큰 숫자는 마지막 세 자리만 계산
                    BigInteger divisor = BigInteger.Pow(10, (int)BigInteger.Log10(tempNum) - 2);
                    displayValue = (double)(tempNum / divisor) / 10.0;
                }

                // 소수점 형식 지정
                if (displayValue >= 100)
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
                else if (displayValue >= 10)
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
                else
                    return $"{displayValue:F2}{standardSuffixes[suffixIndex]}";
            }
            else // 10^36 이상의 알파벳 조합 사용
            {
                double log10 = (double)BigInteger.Log10(number);
                int alphabetOrder = (int)(log10 - 33) / 3;

                char firstChar = (char)('a' + (alphabetOrder / 26));
                char secondChar = (char)('a' + (alphabetOrder % 26));
                string suffix = new string(new[] { firstChar, secondChar });

                // 소수점 계산
                double mantissa = Math.Pow(10, log10 - Math.Floor(log10 / 3) * 3);

                // 소수점 형식 지정 (항상 2자리 소수점 표시)
                return $"{mantissa:F2}{suffix}";
            }
        }
        catch (Exception)
        {
            // 예외 처리: 매우 큰 숫자
            double log10 = (double)BigInteger.Log10(number);

            // 알파벳 조합 계산
            int alphabetOrder = (int)(log10 - 33) / 3;
            if (alphabetOrder >= 0)
            {
                char firstChar = (char)('a' + Math.Min(alphabetOrder / 26, 25));
                char secondChar = (char)('a' + Math.Min(alphabetOrder % 26, 25));
                string suffix = new string(new[] { firstChar, secondChar });

                // 3자리마다 새 접미사, 그 안에서 1.00~999.99 범위 사용
                double mantissa = Math.Pow(10, log10 - Math.Floor(log10 / 3) * 3);

                // 소수점 표시 확보
                return $"{mantissa:F2}{suffix}";
            }
            else
            {
                int suffixIndex = Math.Min((int)(log10 / 3), standardSuffixes.Length - 1);
                return $"{Math.Pow(10, log10 - suffixIndex * 3):F2}{standardSuffixes[suffixIndex]}";
            }
        }
    }

    // 덧셈 연산 - 오버플로우 방지 및 안전한 BigInteger 연산
    public BigInteger Add(BigInteger a, BigInteger b)
    {
        return a + b;
    }

    // 곱셈 연산 - 오버플로우 방지 및 안전한 BigInteger 연산
    public BigInteger Multiply(BigInteger a, BigInteger b)
    {
        return a * b;
    }

    // 비교 연산 - 두 BigInteger 비교
    public bool IsGreaterOrEqual(BigInteger a, BigInteger b)
    {
        return a >= b;
    }
}