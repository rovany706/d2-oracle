using System.Collections.ObjectModel;
using System.Reactive.Linq;
using D2Oracle.Core.Services.DotaKnowledge;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class RoshanItemsDropViewModel : ViewModelBase
{
    public RoshanItemsDropViewModel()
    {
        CurrentItemsImages = new ObservableCollection<string>() { "item_aegis", "item_cheese" };
        LastItemsImages = new ObservableCollection<string>() { "item_aegis" };
    }

    public RoshanItemsDropViewModel(IRoshanItemDropService roshanItemDropService)
    {
        roshanItemDropService.CurrentItems.ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(items => SetItemImages(CurrentItemsImages, items));
        
        roshanItemDropService.LastItems.ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(items =>
            {
                SetItemImages(LastItemsImages, items);
                this.RaisePropertyChanged(nameof(AreLastItemsVisible));
            });
    }

    public ObservableCollection<string> CurrentItemsImages { get; } = [];

    public ObservableCollection<string> LastItemsImages { get; } = [];

    public bool AreLastItemsVisible => LastItemsImages.Any();

    // ReSharper disable once SuggestBaseTypeForParameter
    private static void SetItemImages(ObservableCollection<string> collection, IEnumerable<string> itemNames)
    {
        collection.Clear();

        foreach (var itemName in itemNames)
        {
            collection.Add(itemName);
        }
    }
}