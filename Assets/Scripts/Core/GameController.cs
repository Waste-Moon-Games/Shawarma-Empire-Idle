using UnityEngine;

public class GameController : MonoBehaviour
{
    private CurrencySystem currencySystem;
    private ProductionSystem productionSystem;
    private NPCSpawner npcSpawner;
    private UpgradeSystem upgradeSystem;
    private SaveSystem saveSystem;
    private UIController uiController;
    private VisualProgression visualProgression;

    private void Awake()
    {
        saveSystem = gameObject.AddComponent<SaveSystem>();
        currencySystem = gameObject.AddComponent<CurrencySystem>();
        productionSystem = gameObject.AddComponent<ProductionSystem>();
        npcSpawner = gameObject.AddComponent<NPCSpawner>();
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        uiController = gameObject.AddComponent<UIController>();
        visualProgression = gameObject.AddComponent<VisualProgression>();

        currencySystem.Initialize();
        productionSystem.Initialize(currencySystem);
        npcSpawner.Initialize(productionSystem);
        upgradeSystem.Initialize(currencySystem, productionSystem, npcSpawner);
        visualProgression.Initialize(currencySystem);
        uiController.Initialize(currencySystem, productionSystem, upgradeSystem, visualProgression);

        var data = saveSystem.Load();
        currencySystem.Load(data);
        upgradeSystem.Load(data);
        productionSystem.Load(data);
        npcSpawner.Load(data);
        visualProgression.Load(data);

        uiController.RefreshAll();
    }

    private void OnApplicationQuit() => SaveNow();

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveNow();
    }

    private void SaveNow()
    {
        var data = new SaveData();
        currencySystem.Save(data);
        upgradeSystem.Save(data);
        productionSystem.Save(data);
        npcSpawner.Save(data);
        visualProgression.Save(data);
        saveSystem.Save(data);
    }
}
