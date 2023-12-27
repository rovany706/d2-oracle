using System.Reactive.Linq;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Timers.Roshan;
using Dota2GSI;
using Dota2GSI.Nodes;
using Dota2GSI.Nodes.Events;
using Microsoft.Reactive.Testing;
using NSubstitute;

namespace D2Oracle.Core.Tests.Services.Timers.Roshan;

public class RoshanTimerServiceTests : ReactiveTest
{
    private static GameState CreateGameStateWithRoshanDeathEvent(int roshanDeathGameTime, int roshanDeathClockTime)
    {
        var eventsState = new List<DotaEvent> { GameStateTestHelper.CreateRoshanDeathEvent(roshanDeathGameTime) };

        return GameStateTestHelper.CreateDefaultGameState(roshanDeathGameTime, roshanDeathClockTime) with
        {
            Events = eventsState
        };
    }

    [Test]
    [TestCase(150, 100, 580)]
    [TestCase(126, 110, 590)]
    public void MinRoshanRespawnClockTime_ReturnExpected(int roshanDeathGameTime, int roshanDeathClockTime,
        int expectedSeconds)
    {
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(roshanDeathGameTime, roshanDeathClockTime)));
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);
        var expected = TimeSpan.FromSeconds(expectedSeconds);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.MinRoshanRespawnClockTime, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(150, 100, 760)]
    [TestCase(126, 110, 770)]
    public void MaxRoshanRespawnClockTime_ReturnExpected(int roshanDeathGameTime, int roshanDeathClockTime,
        int expectedSeconds)
    {
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(roshanDeathGameTime, roshanDeathClockTime)));
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);
        var expected = TimeSpan.FromSeconds(expectedSeconds);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.MaxRoshanRespawnClockTime, Is.EqualTo(expected));
    }

    [Test]
    public void RoshanLastDeathClockTime_ByDefault_ReturnNull()
    {
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(Observable.Never<GameState>());

        var sut = new RoshanTimerService(dotaGsiService);

        Assert.That(sut.RoshanLastDeathClockTime, Is.Null);
    }

    [Test]
    public void RoshanLastDeathClockTime_WhenRoshanDied_ReturnDeathClockTime()
    {
        const int expectedSeconds = 169;
        var expected = TimeSpan.FromSeconds(expectedSeconds);
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(200, expectedSeconds)));
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.RoshanLastDeathClockTime, Is.EqualTo(expected));
    }

    [Test]
    public void RoshanLastDeathClockTime_WhenMatchIdChanged_ReturnNull()
    {
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(200, 169)),
            OnNext(20,
                GameStateTestHelper.CreateDefaultGameState() with
                {
                    Map = GameStateTestHelper.CreateDefaultMap() with
                    {
                        Matchid = "111"
                    }
                }));

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.RoshanLastDeathClockTime, Is.Null);
    }

    [Test]
    public void RoshanLastDeathClockTime_WhenRoshanDied_RaiseChanged()
    {
        var isRaised = false;
        var scheduler = new TestScheduler();
        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(200, 169)));
        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        sut.RoshanLastDeathClockTimeChanged += (_, _) => isRaised = true;
        scheduler.Start();

        Assert.That(isRaised, Is.True);
    }

    [Test]
    public void RoshanLastDeathClockTime_AfterMaxRoshanRespawnClockTime_ReturnNull()
    {
        var scheduler = new TestScheduler();

        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(150, 100)),
            OnNext(20, GameStateTestHelper.CreateDefaultGameState(811, 761))
        );

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.RoshanLastDeathClockTime, Is.Null);
    }

    [Test]
    public void RoshanLastDeathClockTime_WhenRoshanDiedBetweenPossibleRespawn_ReturnExpected()
    {
        const int expectedSeconds = 550;
        var expected = TimeSpan.FromSeconds(expectedSeconds);
        var scheduler = new TestScheduler();

        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(150, 100)),
            OnNext(20, CreateGameStateWithRoshanDeathEvent(600, expectedSeconds)));

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        scheduler.Start();

        Assert.That(sut.RoshanLastDeathClockTime, Is.EqualTo(expected));
    }

    [Test]
    public void MinRoshanRespawnTimeReachedEvent_WhenMinRoshanTimeReached_RaisedOnce()
    {
        var eventRaisedTimes = 0;
        var scheduler = new TestScheduler();

        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(150, 100)),
            OnNext(20, GameStateTestHelper.CreateDefaultGameState(631, 581)),
            OnNext(30, GameStateTestHelper.CreateDefaultGameState(632, 582)));

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        sut.MinRoshanRespawnTimeReached += (_, _) => eventRaisedTimes++;
        scheduler.Start();

        Assert.That(eventRaisedTimes, Is.EqualTo(1));
    }

    [Test]
    public void MaxRoshanRespawnTimeReachedEvent_WhenMaxRoshanTimeReached_RaisedOnce()
    {
        var eventRaisedTimes = 0;
        var scheduler = new TestScheduler();

        var observable = scheduler.CreateColdObservable(
            OnNext(10, CreateGameStateWithRoshanDeathEvent(150, 100)),
            OnNext(20, GameStateTestHelper.CreateDefaultGameState(811, 761)),
            OnNext(30, GameStateTestHelper.CreateDefaultGameState(812, 762)));

        var dotaGsiService = Substitute.For<IDotaGsiService>();
        dotaGsiService.GameStateObservable.Returns(observable);

        var sut = new RoshanTimerService(dotaGsiService);
        sut.MaxRoshanRespawnTimeReached += (_, _) => eventRaisedTimes++;
        scheduler.Start();

        Assert.That(eventRaisedTimes, Is.EqualTo(1));
    }
}