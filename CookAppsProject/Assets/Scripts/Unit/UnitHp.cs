using UnityEngine;
using UnityEngine.UI;

public class UnitHp : MonoBehaviour
{
    [SerializeField] private Slider _hpBarSlider;

    private int _hp;
    [HideInInspector]
    public int MaxHp;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value < 0 ? 0 : value;
        }
    }

    private void Start()
    {
        Hp = MaxHp;
    }

    public void ChangeHp(int damage)
    {
        Hp -= damage;
        UpdateHpBarUI();
    }

    private void UpdateHpBarUI()
    {
        _hpBarSlider.value = (float)Hp / MaxHp;
    }

    public bool IsDie()
    {
        return Hp <= 0;
    }

    public void FlipXSlider(float x)
    {
        _hpBarSlider.transform.localScale = new Vector3(x, 1, 1);
    }
}
