using System.Reactive;
using D2Oracle.Core.Services;
using Dota2GSI;
using Microsoft.Reactive.Testing;
using NSubstitute;

namespace D2Oracle.Core.Tests.Services;

public class GameStateObserverTests : ReactiveTest
{
    private class TestGameStateObserver : GameStateObserver
    {
        public TestGameStateObserver(IDotaGsiService dotaGsiService) : base(dotaGsiService)
        {
        }

        public bool IsOnCurrentMatchIdChangedCalled { get; private set; }

        protected override void OnCurrentMatchIdChanged()
        {
            IsOnCurrentMatchIdChangedCalled = true;
        }
    }

    [Test]
    public void Constructor_SubscribeToGameStateObservable()
    {
        var scheduler = new TestScheduler();
        var input = scheduler.CreateColdObservable(Array.Empty<Recorded<Notification<GameState>>>());

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(input);

        _ = new TestGameStateObserver(dotaGsiService);

        Assert.That(input.Subscriptions, Is.Not.Empty);
    }

    [Test]
    public void ProcessGameState_WhenMatchIdChanged_CallOnCurrentMatchIdChanged()
    {
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(OnNext(10, GameStateTestHelper.CreateDefaultGameState()));
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new TestGameStateObserver(dotaGsiService);
        
        scheduler.Start();
        
        Assert.That(sut.IsOnCurrentMatchIdChangedCalled);
    }
}