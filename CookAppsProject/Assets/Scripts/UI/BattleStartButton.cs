using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleStartButton : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnRestart += ButtonActive;
    }

    private void OnDisable()
    {
        GameManager.instance.OnRestart -= ButtonActive;
    }

    public void StartBtn()
    {
        GameManager.instance.OnBattle?.Invoke();
        GetComponent<Button>().interactable = false;
    }

    private void ButtonActive()
    {
        GetComponent<Button>().interactable = true;
    }
}
