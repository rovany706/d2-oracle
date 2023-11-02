using System.Reactive.Linq;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Timers.Runes;
using Dota2GSI;
using Microsoft.Reactive.Testing;
using NSubstitute;

namespace D2Oracle.Core.Tests.Services.Timers.Runes;

public class WisdomRuneTimerServiceTests : ReactiveTest
{
    [Test]
    public void WisdomRuneSpawnTimeMultiplierInMinutes_ReturnExpected()
    {
        const int expected = 7;
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(Observable.Never<GameState>());

        var sut = new WisdomRuneTimerService(dotaGsiService);

        Assert.That(sut.WisdomRuneSpawnTimeMultiplierInMinutes, Is.EqualTo(expected));
    }

    [Test]
    public void WisdomRuneSpawnsSoonEvent_WhenClockTimeIsBetweenNotificationAndRespawn_RaisedOnce()
    {
        var eventRaisedTimes = 0;
        var testScheduler = new TestScheduler();

        var observable = testScheduler.CreateColdObservable(
            OnNext(10, GameStateTestHelper.CreateDefaultGameState(200, 150)),
            OnNext(20, GameStateTestHelper.CreateDefaultGameState(410, 360)),
            OnNext(30, GameStateTestHelper.CreateDefaultGameState(430, 380)), // 6:20 rune expected
            OnNext(40, GameStateTestHelper.CreateDefaultGameState(440, 390)) // no second event fired
        );

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new WisdomRuneTimerService(dotaGsiService);
        sut.WisdomRuneSpawnsSoon += (_, _) => eventRaisedTimes++;
        testScheduler.Start();

        Assert.That(eventRaisedTimes, Is.EqualTo(1));
    }

    [Test]
    public void WisdomRuneSpawnsSoonEvent_WhenTwoRunesExpected_RaisedTwice()
    {
        var eventRaisedTimes = 0;
        var testScheduler = new TestScheduler();

        var observable = testScheduler.CreateColdObservable(
            OnNext(10, GameStateTestHelper.CreateDefaultGameState(200, 150)),
            OnNext(20, GameStateTestHelper.CreateDefaultGameState(410, 360)),
            OnNext(30, GameStateTestHelper.CreateDefaultGameState(430, 380)), // 6:20 rune expected
            OnNext(40, GameStateTestHelper.CreateDefaultGameState(440, 390)), // no second event fired
            OnNext(50, GameStateTestHelper.CreateDefaultGameState(830, 780)),
            OnNext(60, GameStateTestHelper.CreateDefaultGameState(896, 796)), // 13:16 rune expected
            OnNext(70, GameStateTestHelper.CreateDefaultGameState(850, 800)), // no second event fired
            OnNext(80, GameStateTestHelper.CreateDefaultGameState(900, 850))
        );

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new WisdomRuneTimerService(dotaGsiService);
        sut.WisdomRuneSpawnsSoon += (_, _) => eventRaisedTimes++;
        testScheduler.Start();

        Assert.That(eventRaisedTimes, Is.EqualTo(2));
    }
}