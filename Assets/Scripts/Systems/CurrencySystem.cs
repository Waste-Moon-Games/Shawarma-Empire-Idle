using System;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public float Money { get; private set; }
    public event Action<float> OnMoneyChanged;

    public void Initialize() => Money = 0f;

    public void AddMoney(float value)
    {
        Money += value;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool SpendMoney(float value)
    {
        if (Money < value) return false;
        Money -= value;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }

    public void Save(SaveData data) => data.money = Money;
    public void Load(SaveData data) => Money = data.money;
}
