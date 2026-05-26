using System;
using UnityEngine;

public class ProductionSystem : MonoBehaviour
{
    private CurrencySystem currency;

    public float CookTime { get; private set; } = 2.5f;
    public float IncomePerSale { get; private set; } = 5f;
    public int AutoCookLevel { get; private set; }
    public float Progress { get; private set; }
    public int PendingShawarmas { get; private set; }

    public event Action<float> OnProgress;
    public event Action OnStockChanged;

    public void Initialize(CurrencySystem currencySystem)
    {
        currency = currencySystem;
    }

    private void Update()
    {
        if (AutoCookLevel <= 0) return;

        Progress += Time.deltaTime / CookTime;
        OnProgress?.Invoke(Mathf.Clamp01(Progress));

        while (Progress >= 1f)
        {
            Progress -= 1f;
            PendingShawarmas++;
            OnStockChanged?.Invoke();
            SpawnFloatingText("+Шаурма", new Vector3(0, -2.5f, 0), Color.yellow);
        }
    }

    public void ManualCookClick()
    {
        PendingShawarmas++;
        OnStockChanged?.Invoke();
        SpawnFloatingText("+1", new Vector3(0, -2.5f, 0), Color.white);
    }

    public bool TrySellOne()
    {
        if (PendingShawarmas <= 0) return false;

        PendingShawarmas--;
        currency.AddMoney(IncomePerSale);
        OnStockChanged?.Invoke();
        SpawnFloatingText($"+${IncomePerSale:0}", new Vector3(2f, -1.5f, 0), Color.green);
        return true;
    }

    public void UpgradeSpeed() => CookTime = Mathf.Max(0.4f, CookTime * 0.88f);
    public void UpgradeIncome() => IncomePerSale *= 1.2f;
    public void UpgradeAutoCook() => AutoCookLevel++;

    public void Save(SaveData data) => data.autoCookLevel = AutoCookLevel;

    public void Load(SaveData data)
    {
        for (int i = 0; i < data.speedLevel; i++) UpgradeSpeed();
        for (int i = 0; i < data.incomeLevel; i++) UpgradeIncome();
        AutoCookLevel = data.autoCookLevel;
    }

    private void SpawnFloatingText(string text, Vector3 position, Color color)
    {
        var go = new GameObject("FloatText");
        go.transform.position = position;

        var mesh = go.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.characterSize = 0.12f;
        mesh.color = color;

        go.AddComponent<FloatingText>();
    }
}
