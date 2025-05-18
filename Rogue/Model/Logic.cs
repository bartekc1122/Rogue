using System.Diagnostics.Tracing;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Rogue;

public class Logic
{
    public bool GameEnd = false;
    private GameState _gameState;
    private EntityManager _entityManager => _gameState.EntityManager;
    private Player _player = null!;

    public void SelectPlayer(int index)
    {
        _player = _gameState.Players[index];
    }
    public void AddPlayer(int index)
    {
        var player = new Player();
        _entityManager.AddEntity(player, new Point(1, 2*index+1));
        _gameState.Players.Add(index, player);
    }
    public void DeletePlayer(int index)
    {
        _entityManager.RemoveEntity(_gameState.Players[index]);
        _gameState.Players.Remove(index);
    }

    public Logic(GameState gameState)
    {
        _gameState = gameState;
        // _player = player;
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
    public void MoveCursorUp()
    {
        _player.Inventory.MoveCursor(1);
    }
    public void MoveCursorDown()
    {
        _player.Inventory.MoveCursor(-1);
    }
    public void AttackSelect()
    {
        _player.ChoseAttackIndex = (_player.ChoseAttackIndex + 1) % _player.Attacks.Count;
    }
    public string WSAD(ConsoleKey? key)
    {
        Point newPosition = _gameState.EntityManager.GetEntityPosition(_player);
        switch (key as ConsoleKey?)
        {
            case ConsoleKey.W:
                newPosition.Y--;
                var enemy = _gameState.EntityManager.MoveEntity(_player, newPosition);
                if (enemy != null)
                {
                    return Fight(_player, enemy);
                }
                return "Moved down";
            case ConsoleKey.S:
                newPosition.Y++;
                enemy = _gameState.EntityManager.MoveEntity(_player, newPosition);
                if (enemy != null)
                {
                    return Fight(_player, enemy);
                }
                return "Moved up";
            case ConsoleKey.A:
                newPosition.X--;
                enemy = _gameState.EntityManager.MoveEntity(_player, newPosition);
                if (enemy != null)
                {
                    return Fight(_player, enemy);
                }
                return "Moved left";
            case ConsoleKey.D:
                newPosition.X++;
                enemy = _gameState.EntityManager.MoveEntity(_player, newPosition);
                if (enemy != null)
                {
                    return Fight(_player, enemy);
                }
                return "Moved right";
            default:
                return string.Empty;
        }
    }
}