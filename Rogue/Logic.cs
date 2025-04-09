using System.Diagnostics.Tracing;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Rogue;

public class Logic
{
    public bool GameEnd = false;
    private GameState _gameState;
    private EntityManager _entityManager => _gameState.EntityManager;
    private Player _player => _gameState.Player;

    public Logic(GameState gameState)
    {
        _gameState = gameState;
    }
    public bool TryPickUpItem()
    {
        var items = _entityManager.GetItemsAt(_entityManager.GetEntityPosition(_player));
        if (items.Count() == 0)
        { return false; }
        var topitem = items.First();
        _entityManager.RemoveEntity(topitem);
        _player.Inventory.AddToInventory(topitem);
        topitem.ApplyOnPickUp(_player);
        return true;
    }
    public bool DrinkLogic()
    {
        var potion = _player.Inventory.GetSelectedItem();
        if (potion == null)
        {
            return false;
        }
        var res = potion.Drink(_player);
        if (res == false)
        {
            return false;
        }
        _player.Inventory.RemoveFromInventory(potion);
        return true;
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
    public void TryThrowAllItems()
    {
        var inventoryItems = _player.Inventory.GetInventory();
        var items = new List<IItem>(inventoryItems);
        for (int i = 0; i < items.Count(); i++)
        {
            var item = items[i];
            if (item == null)
            {
                return;
            }
            _entityManager.AddEntity(item, _entityManager.GetEntityPosition(_player));
            _player.Inventory.RemoveFromInventory(item);
            item.ApplyOnDePickUp(_player);
        }
        return;
    }
    public bool EquipRight()
    {
        var item = _player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (_player.Hands.RightEquip(item))
            {
                _player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(_player);
                return true;
            }
        }
        var iteme = _player.Hands.RightUnequip();
        if (iteme != null)
        {
            _player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(_player);
            return false;
        }
        return false;
    }
    public bool EquipLeft()
    {
        var item = _player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (_player.Hands.LeftEquip(item))
            {
                _player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(_player);
                return true;
            }
        }
        var iteme = _player.Hands.LeftUnequip();
        if (iteme != null)
        {
            _player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(_player);
        }
        return false;
    }
}