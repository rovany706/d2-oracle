using System.Diagnostics;
using System.Reactive.Subjects;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services.Timers.Roshan;
using Dota2GSI;
using Dota2GSI.Nodes.Events;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class RoshanItemDropService : GameStateObserver, IRoshanItemDropService
{
    private int deathCount = 2;
    private readonly Subject<IEnumerable<string>> itemsValues = new();
    private readonly RoshanDeathItemProvider itemProvider = new();
    private bool isDaytime;

    public RoshanItemDropService(IDotaGsiService dotaGsiService) : base(dotaGsiService)
    {
    }

    public IObservable<IEnumerable<string>> Items => this.itemsValues;
    
    public IEnumerable<string> GetCurrentItems()
    {
        return GetCurrentRoshanItems();
    }

    private bool IsDaytime
    {
        get => this.isDaytime;
        set
        {
            if (this.isDaytime == value)
            {
                return;
            }
            
            this.isDaytime = value;
            PushNewItems();
        }
    }

    protected override void ProcessGameState(GameState? gameState)
    {
        base.ProcessGameState(gameState);

        if (gameState?.Map is null || !gameState.IsInGame())
        {
            return;
        }
        
        IsDaytime = gameState.Map.Daytime;
        
        var roshanDeathEvent = GetRoshanDeathEvent(gameState);
        if (roshanDeathEvent is not null)
        {
            this.deathCount++;
            PushNewItems();
        }
    }
    
    protected override void OnCurrentMatchIdChanged()
    {
        this.deathCount = 2;
    }

    private void PushNewItems()
    {
        var currentRoshanItems = GetCurrentRoshanItems();
        Debug.Write($"SET NEW ITEMS ");
        foreach (var currentRoshanItem in currentRoshanItems)
        {
            Debug.Write(currentRoshanItem);
        }
        Debug.WriteLine("");

        this.itemsValues.OnNext(currentRoshanItems);
    }

    private IReadOnlyList<string> GetCurrentRoshanItems()
    {
        var index = Math.Clamp(this.deathCount, 0, this.itemProvider.RoshanItemDrops.Count);
        var itemDrops = this.itemProvider.RoshanItemDrops[index];
        
        return IsDaytime ? itemDrops.DayItems : itemDrops.NightItems;
    }

    private static DotaEvent? GetRoshanDeathEvent(GameState gameState)
    {
        return gameState.Events.SingleOrDefault(x => x.EventType == DotaEventType.RoshanKilled);
    }
}