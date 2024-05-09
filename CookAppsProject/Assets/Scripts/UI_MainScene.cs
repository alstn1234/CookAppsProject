using TMPro;
using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    private GameObject BattleEndPopup;

    private void Awake()
    {
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BattleEndPopup = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PopupCanvas"));
        BattleEndPopup.SetActive(false);
        GameManager.instance.BattleEndPopup = this.BattleEndPopup;
        GameManager.instance.resultText = BattleEndPopup.GetComponentInChildren<TextMeshProUGUI>();
    }
}
