using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using D2Oracle.Core.GSI;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.ViewModels;

public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            HandleActivation();
            Disposable
                .Create(HandleDeactivation)
                .DisposeWith(disposables);
        });
    }

    private void HandleActivation()
    {
        Listener = new GSIListener();
        Listener.GameStateListener.Start();
        Debug.WriteLine(Listener.GameStateListener.Running ? ":)" : ":(");
        Listener.GameStateListener.NewGameState += GameStateListenerOnNewGameState;
    }

    private void HandleDeactivation()
    {
        Listener.Dispose();
    }

    private void GameStateListenerOnNewGameState(GameState gameState)
    {
        Debug.WriteLine("got state");
    }

    public string Greeting => "Welcome to Avalonia!";

    public GSIListener Listener { get; set; }

    public ViewModelActivator Activator { get; }
}