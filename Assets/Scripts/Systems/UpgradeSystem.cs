using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private CurrencySystem currency;
    private ProductionSystem production;
    private NPCSpawner npcSpawner;

    public int SpeedLevel { get; private set; }
    public int IncomeLevel { get; private set; }
    public int AutoCookLevel { get; private set; }
    public int SpawnRateLevel { get; private set; }

    public void Initialize(CurrencySystem currencySystem, ProductionSystem productionSystem, NPCSpawner spawner)
    {
        currency = currencySystem;
        production = productionSystem;
        npcSpawner = spawner;
    }

    public float SpeedCost() => 25f * Mathf.Pow(1.6f, SpeedLevel);
    public float IncomeCost() => 40f * Mathf.Pow(1.7f, IncomeLevel);
    public float AutoCost() => 80f * Mathf.Pow(1.8f, AutoCookLevel);
    public float SpawnCost() => 60f * Mathf.Pow(1.6f, SpawnRateLevel);

    public bool BuySpeed()
    {
        if (!currency.SpendMoney(SpeedCost())) return false;
        SpeedLevel++;
        production.UpgradeSpeed();
        return true;
    }

    public bool BuyIncome()
    {
        if (!currency.SpendMoney(IncomeCost())) return false;
        IncomeLevel++;
        production.UpgradeIncome();
        return true;
    }

    public bool BuyAuto()
    {
        if (!currency.SpendMoney(AutoCost())) return false;
        AutoCookLevel++;
        production.UpgradeAutoCook();
        return true;
    }

    public bool BuySpawn()
    {
        if (!currency.SpendMoney(SpawnCost())) return false;
        SpawnRateLevel++;
        npcSpawner.UpgradeSpawnRate();
        return true;
    }

    public void Save(SaveData data)
    {
        data.speedLevel = SpeedLevel;
        data.incomeLevel = IncomeLevel;
        data.autoCookLevel = AutoCookLevel;
        data.spawnRateLevel = SpawnRateLevel;
    }

    public void Load(SaveData data)
    {
        SpeedLevel = data.speedLevel;
        IncomeLevel = data.incomeLevel;
        AutoCookLevel = data.autoCookLevel;
        SpawnRateLevel = data.spawnRateLevel;
    }
}
