using UnityEngine;

public class EndPopupOkButton : MonoBehaviour
{
    public void BattleEndBtn()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        GameManager.instance.OnRestart?.Invoke();
    }
}
