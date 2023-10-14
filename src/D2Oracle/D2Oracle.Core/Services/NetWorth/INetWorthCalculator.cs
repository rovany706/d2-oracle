using Dota2GSI;

namespace D2Oracle.Core.Services.NetWorth;

public interface INetWorthCalculator
{
    uint Calculate(GameState? gameState);
}