using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private ProductionSystem production;
    public float SpawnInterval { get; private set; } = 3f;
    private float timer;

    public void Initialize(ProductionSystem productionSystem)
    {
        production = productionSystem;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < SpawnInterval) return;

        timer = 0f;
        SpawnNpc();
    }

    private void SpawnNpc()
    {
        var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        npc.transform.position = new Vector3(-8f, -2.3f, 0f);
        npc.transform.localScale = new Vector3(0.6f, 1.3f, 0.6f);

        var buyer = npc.AddComponent<NPCBuyer>();
        buyer.Initialize(production);
    }

    public void UpgradeSpawnRate() => SpawnInterval = Mathf.Max(0.75f, SpawnInterval * 0.9f);
    public void Save(SaveData data) { }

    public void Load(SaveData data)
    {
        for (int i = 0; i < data.spawnRateLevel; i++) UpgradeSpawnRate();
    }
}

public class NPCBuyer : MonoBehaviour
{
    private enum State { Enter, Exit }

    private State state;
    private ProductionSystem production;

    public void Initialize(ProductionSystem productionSystem)
    {
        production = productionSystem;
        state = State.Enter;
    }

    private void Update()
    {
        var target = state == State.Enter ? new Vector3(2f, -2.3f, 0f) : new Vector3(9f, -2.3f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, target, 3.5f * Time.deltaTime);

        if (state == State.Enter && Vector3.Distance(transform.position, target) < 0.01f)
        {
            production.TrySellOne();
            state = State.Exit;
        }

        if (state == State.Exit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
