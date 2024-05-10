using TMPro;
using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    private GameObject _battleEndPopup;
    [SerializeField] private TextMeshProUGUI _StageText;

    private void Awake()
    {
    }

    private void Start()
    {
        Init();
        GameManager.instance.OnUpdateStage += UpdateStageUI;
    }

    private void OnDisable()
    {
        GameManager.instance.OnUpdateStage -= UpdateStageUI;
    }

    private void Init()
    {
        _battleEndPopup = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PopupCanvas"));
        _battleEndPopup.SetActive(false);
        GameManager.instance.BattleEndPopup = this._battleEndPopup;
        GameManager.instance.resultText = _battleEndPopup.GetComponentInChildren<TextMeshProUGUI>();
        UpdateStageUI();
    }

    private void UpdateStageUI()
    {
        _StageText.text = $"스테이지 {GameManager.instance.Stage}";
    }
}
