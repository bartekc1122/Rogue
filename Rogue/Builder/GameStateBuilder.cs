namespace Rogue;
public class GameStateBuilder
{
    private GameState _gameState; 
    private List<IBuildProcedure> _porcedures = new List<IBuildProcedure>();
    public GameStateBuilder()
    {
        _gameState = new GameState();
    }
    public void StartEmptyMap()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
            for (int x = 0; x < Constants.MapWidth; x++)
                    _gameState.Map[y, x] = TerrainType.Floor;
    } 
    public void StartFilledMap()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
            for (int x = 0; x < Constants.MapWidth; x++)
                    _gameState.Map[y, x] = TerrainType.Wall;
    }
    public void AddProcedure(IBuildProcedure procedure)
    {
        _porcedures.Add(procedure);
    }
    public GameState Build()
    {
        StartEmptyMap();
        StartFilledMap();
        foreach(var procedure in _porcedures)
        {
            procedure.Apply(_gameState);
        }
        return _gameState;
    }
}