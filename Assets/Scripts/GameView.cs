// GameView.cs - UI�� �����ϴ� ��
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
        // ��� ���� ���
        _viewModel = GetComponent<GameViewModel>();

        // �̺�Ʈ ����
        _viewModel.OnUIUpdateRequired += UpdateUI;

        // ��ư �̺�Ʈ ����
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
            
            // 100�� Ŭ�� ȿ�� ����
            _viewModel.HandleMultiClick(100);
            // UI ������Ʈ
            UpdateUI();
        });

        // �ʱ� UI ������Ʈ
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"����: {_viewModel.FormattedScore}";
        clickValueText.text = $"Ŭ����: {_viewModel.FormattedClickValue}";
        upgradeButtonText.text = $"���׷��̵� (x10)\n���: {_viewModel.GetUpgradeCost()}";
    }

    private IEnumerator ButtonClickEffect(Button button)
    {
        // ��ư Ŭ�� ȿ�� (������ ������ �ִϸ��̼�)
        var originalScale = button.transform.localScale;
        button.transform.localScale = originalScale * 1.1f;

        yield return new WaitForSeconds(0.1f);

        button.transform.localScale = originalScale;
    }
}