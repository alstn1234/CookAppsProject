using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMethod : MonoBehaviour
{
    public void StartBtn()
    {
        GameManager.instance.OnBattle?.Invoke();
        GetComponent<Button>().interactable = false;
    }

    public void BattleEndBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
        GameManager.instance.ReadyMode();
    }
}
