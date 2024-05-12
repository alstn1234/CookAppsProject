using UnityEngine;

public class PopupButton : MonoBehaviour
{
    public void BattleEndBtn()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        GameManager.instance.OnRestart?.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
