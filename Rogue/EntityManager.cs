using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Rogue;

public class EntityManager
{
    private readonly List<IEntity>[,] _entityGrid;
    private readonly List<IEntity> _allEntities = new();
    private readonly GameState _state;

    public EntityManager(GameState state)
    {
        _state = state;
        _entityGrid = new List<IEntity>[Constants.MapHeight, Constants.MapWidth];
        for (int y = 0; y < Constants.MapHeight; y++)
            for (int x = 0; x < Constants.MapWidth; x++)
                _entityGrid[y, x] = new List<IEntity>();
    }
    public void AddEntity(IEntity entity, Point position)
    {
        if (!IsPositionValid(position))
            return;
        entity.Position = position;
        _entityGrid[position.Y, position.X].Add(entity);
        _allEntities.Add(entity);
    }
    public void RemoveEntity(IEntity entity)
    {
        var position = GetEntityPosition(entity);
        entity.Position = null;
        _entityGrid[position.Y, position.X].Remove(entity);
        _allEntities.Remove(entity);
    }
    public void MoveEntity(IEntity entity, Point newPostition)
    {
        if (!IsPositionValid(newPostition))
            return;
        if (entity.Position == null)
            return;
        Point oldPostition = entity.Position.Value;
        _entityGrid[oldPostition.Y, oldPostition.X].Remove(entity);

        entity.Position = newPostition;
        _entityGrid[newPostition.Y, newPostition.X].Add(entity);
    }
    public List<IEntity> GetEntitesAt(Point position)
    {
        return _entityGrid[position.Y, position.X];
    }
    public List<IItem> GetItemsAt(Point position)
    {
        List<IItem> items = GetEntitesAt(position).OfType<IItem>().ToList();
        return items;
    }

    public Point GetEntityPosition(IEntity entitiy)
    {
        return entitiy.Position!.Value;
    }

    private bool IsPositionValid(Point position)
    {
        return position.X > 0 && position.X < Constants.MapWidth
        && position.Y > 0 && position.Y < Constants.MapHeight && _state.Map[position.Y, position.X] != TerrainType.Wall;
    }
    public List<IEntity> GetAllEntities()
    {
        return _allEntities;
    }
}