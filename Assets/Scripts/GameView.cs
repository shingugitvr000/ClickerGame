// GameView.cs - UI를 관리하는 뷰
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI clickValueText;
    [SerializeField] private Button multiClickButton;
    [SerializeField] private Button clickButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private TextMeshProUGUI multiClickButtonText;

    private GameViewModel _viewModel;

    void Start()
    {
        // 뷰모델 참조 얻기
        _viewModel = GetComponent<GameViewModel>();

        // 이벤트 구독
        _viewModel.OnUIUpdateRequired += UpdateUI;

        // 버튼 이벤트 연결
        clickButton.onClick.AddListener(() => {
            _viewModel.HandleClick();
            StartCoroutine(ButtonClickEffect(clickButton));
        });

        upgradeButton.onClick.AddListener(() => {
            if (_viewModel.TryUpgradeClick())
            {
                StartCoroutine(ButtonClickEffect(upgradeButton));
            }
        });

        multiClickButton.onClick.AddListener(() => {
            
            // 100번 클릭 효과 실행
            _viewModel.HandleMultiClick(100);
            // UI 업데이트
            UpdateUI();
        });

        // 초기 UI 업데이트
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"점수: {_viewModel.FormattedScore}";
        clickValueText.text = $"클릭당: {_viewModel.FormattedClickValue}";
        upgradeButtonText.text = $"업그레이드 (x10)\n비용: {_viewModel.GetUpgradeCost()}";
    }

    private IEnumerator ButtonClickEffect(Button button)
    {
        // 버튼 클릭 효과 (간단한 스케일 애니메이션)
        var originalScale = button.transform.localScale;
        button.transform.localScale = originalScale * 1.1f;

        yield return new WaitForSeconds(0.1f);

        button.transform.localScale = originalScale;
    }
}