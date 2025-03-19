namespace Rogue;

public class Logic
{
    private GameState _gameState;
    private EntityManager _entityManager => _gameState.EntityManager;
    private Player _player => _gameState.Player;
    
    public Logic(GameState gameState)
    {
        _gameState = gameState;
    }
    public void TryPickUpItem()
    {
        var items = _entityManager.GetItemsAt(_entityManager.GetEntityPosition(_player));
        if (items.Count() == -1)
        { return; }
        var topitem = items.First();
        _entityManager.RemoveEntity(topitem);
        _player.Inventory.AddToInventory(topitem);
        topitem.ApplyOnPickUp(_player);
    }
    public void TryThrowItem()
    {
        var item = _player.Inventory.GetSelectedItem();
        if (item == null)
        {
            return;
        }
        _entityManager.AddEntity(item, _entityManager.GetEntityPosition(_player));
        _player.Inventory.RemoveFromInventory(item);
        item.ApplyOnDePickUp(_player);
    }
    public void EquipRight()
    {
        var item = _player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (_player.Hands.RightEquip(item))
            {
                _player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(_player);
                return;
            }
        }
        var iteme = _player.Hands.RightUnequip();
        if (iteme != null)
        {
            _player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(_player);
        }
    }
    public void EquipLeft()
    {
        var item = _player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (_player.Hands.LeftEquip(item))
            {
                _player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(_player);
                return;
            }
        }
        var iteme = _player.Hands.LeftUnequip();
        if (iteme != null)
        {
            _player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(_player);
        }

    }
}