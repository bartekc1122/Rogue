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
        var topItem = items.First();
        _entityManager.RemoveEntity(topItem);
        _player.Inventory.AddToInventory(topItem);
        topItem.ApplyOnPickUp(_player);
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
        var itemUnequip = _player.Hands.LeftUnequip();
        if (itemUnequip != null)
        {
            _player.Inventory.AddToInventory(itemUnequip);
            itemUnequip.ApplyOnDeHanded(_player);
        }
        return false;
    }
    public string Fight(Player player, IMonster monster)
    {
        var attack = player.Attacks[player.ChoseAttackIndex];
        int damageDealt = 0;
        int playerDefense = 0;
        if (player.Hands.Right != null)
        {
            var accept = player.Hands.Right.Accept(attack, player);
            damageDealt += accept.damage;
            playerDefense += accept.defense;
        }
        if (player.Hands.Left != null && player.Hands.Left != player.Hands.Right)
        {
            var accept = player.Hands.Left.Accept(attack, player);
            damageDealt += accept.damage;
            playerDefense += accept.defense;
        }
        var damage = damageDealt - monster.Defense < 0 ? 0 : damageDealt - monster.Defense;
        monster.Health -= damage;
        if (monster.Health <= 0)
        {
            _gameState.EntityManager.RemoveEntity(monster);
            return $"{damage} damage kills {monster.Name}";
        }
        int received = monster.Damage - playerDefense < 0 ? 0 : monster.Damage - playerDefense;
        player.Stats.Health -= received;
        return $"Dealt: {damage} to {monster.Name}, Received: {received}";
    }
}