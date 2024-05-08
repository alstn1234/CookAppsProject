using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public void StartBtn()
    {
        GameManager.instance.OnBattle?.Invoke();
        GetComponent<Button>().interactable = false;
    }
}
