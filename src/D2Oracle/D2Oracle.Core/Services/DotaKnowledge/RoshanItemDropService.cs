using System.Reactive.Subjects;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services.Timers.Roshan;
using Dota2GSI;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class RoshanItemDropService : GameStateObserver, IRoshanItemDropService
{
    private readonly IRoshanDeathObserverService roshanDeathObserverService;
    private readonly Subject<IEnumerable<string>> currentItemsValues = new();
    private readonly Subject<IEnumerable<string>> lastItemsValues = new();
    private readonly RoshanDeathItemProvider itemProvider = new();
    private bool isDaytime;

    public RoshanItemDropService(IRoshanDeathObserverService roshanDeathObserverService, IDotaGsiService dotaGsiService)
        : base(dotaGsiService)
    {
        this.roshanDeathObserverService = roshanDeathObserverService;

        this.roshanDeathObserverService.IsRoshanAliveChanged += OnIsRoshanAliveChanged;
    }

    private void OnIsRoshanAliveChanged(object? sender, EventArgs e)
    {
        // Roshan died
        if (this.roshanDeathObserverService.IsRoshanAlive == false)
        {
            PushNewCurrentItems();
            PushNewLastItems();
        }
    }

    public IObservable<IEnumerable<string>> CurrentItems => this.currentItemsValues;
    public IObservable<IEnumerable<string>> LastItems => this.lastItemsValues;

    public IEnumerable<string> GetCurrentItems()
    {
        return GetRoshanItems(this.roshanDeathObserverService.RoshanDeathCount);
    }
    
    public IEnumerable<string> GetLastItems()
    {
        return GetRoshanItems(this.roshanDeathObserverService.RoshanDeathCount - 1);
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
            OnIsDaytimeChanged();
        }
    }

    private void OnIsDaytimeChanged()
    {
        PushNewCurrentItems();
    }

    protected override void ProcessGameState(GameState? gameState)
    {
        base.ProcessGameState(gameState);

        if (gameState?.Map is null || !gameState.IsInGame())
        {
            return;
        }

        IsDaytime = gameState.Map.Daytime;
    }

    protected override void OnCurrentMatchIdChanged()
    {
        // Reset items
        this.currentItemsValues.OnNext(GetRoshanItems(0));
        this.lastItemsValues.OnNext(Array.Empty<string>());
    }

    private void PushNewCurrentItems()
    {
        var currentRoshanItems = GetCurrentItems();
        this.currentItemsValues.OnNext(currentRoshanItems);
    }

    private void PushNewLastItems()
    {
        // Roshan didn't die, so no items dropped before
        if (this.roshanDeathObserverService.RoshanDeathCount == 0)
        {
            this.lastItemsValues.OnNext(Array.Empty<string>());
        
            return;
        }

        var lastItems = GetLastItems();
        this.lastItemsValues.OnNext(lastItems);
    }

    private IEnumerable<string> GetRoshanItems(int roshanDeathCount)
    {
        var index = Math.Clamp(roshanDeathCount, 0, this.itemProvider.RoshanItemDrops.Count - 1);
        var itemDrops = this.itemProvider.RoshanItemDrops[index];

        return IsDaytime ? itemDrops.DayItems : itemDrops.NightItems;
    }
}