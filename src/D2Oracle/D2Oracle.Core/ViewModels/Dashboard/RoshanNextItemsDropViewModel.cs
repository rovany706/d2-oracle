using System.Collections.ObjectModel;
using System.Reactive.Linq;
using D2Oracle.Core.Services.DotaKnowledge;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class RoshanNextItemsDropViewModel : ViewModelBase
{
    public RoshanNextItemsDropViewModel()
    {
    }

    public RoshanNextItemsDropViewModel(IRoshanItemDropService roshanItemDropService)
    {
        roshanItemDropService.Items.ObserveOn(RxApp.MainThreadScheduler).Subscribe(SetItemImages);
        //SetItemImages(roshanItemDropService.GetCurrentItems());
    }

    public ObservableCollection<string> ItemsImages { get; } = [];
    
    private void SetItemImages(IEnumerable<string> itemNames)
    {
        ItemsImages.Clear();
        
        foreach (var itemName in itemNames)
        {
            ItemsImages.Add(itemName);
        }
    }
}