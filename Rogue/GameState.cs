
using Rogue;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
public enum TerrainType
{
    Wall = '█',
    Floor = ' '
}
public class Constants
{
    public const int MapWidth = 40;
    public const int MapHeight = 20;
}
public class GameState
{
    public TerrainType[,] Map { get; }
    public Player Player { get; set; }
    public EntityManager EntityManager;
    public GameState()
    {
        Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
        InitializeMap();
        Player = new Player();
        EntityManager = new EntityManager(this);
        EntityManager.AddEntity(Player, new Point(1, 1));
        EntityManager.AddEntity(new Weapon("Sword", 't', ConsoleColor.DarkMagenta, 10), new Point(24, 4));
        EntityManager.AddEntity(new Item("Bread", 'B', ConsoleColor.Yellow), new Point(5, 6));
        EntityManager.AddEntity(new Item("Duck", 'D', ConsoleColor.Yellow), new Point(5, 6));
        EntityManager.AddEntity(new Item("Hoe", 'H', ConsoleColor.Gray), new Point(25, 14));
        EntityManager.AddEntity(new Weapon("Double Sword", 'T', ConsoleColor.Cyan, 20, true), new Point(37, 7));
        EntityManager.AddEntity(new Mony("Coin", 'o', ConsoleColor.Yellow, false, 1), new Point(10, 18));
        EntityManager.AddEntity(new Mony("Goldn Coin", 'O', ConsoleColor.Yellow, false, 10), new Point(10, 10));

        var sword = new Weapon("Sword", 't', ConsoleColor.DarkBlue, 10);
        var cursedSwod = new CursedWeapon(sword);
        var agrocusSwod = new AggroWeapon(cursedSwod);
        EntityManager.AddEntity(agrocusSwod, new Point(1, 5));

        var dupa = new Weapon("Duupa", 'D', ConsoleColor.DarkRed, 50, isTwoHanded: true);
        var przeklentadupa = new CursedWeapon(dupa);
        EntityManager.AddEntity(przeklentadupa, new Point(8, 10));
    }
    private void InitializeMap()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
        {
            for (int x = 0; x < Constants.MapWidth; x++)
            {
                if (y == 0 || x == 0 || y == Constants.MapHeight - 1 || x == Constants.MapWidth - 1)
                {
                    Map[y, x] = TerrainType.Wall;
                }
                else
                {
                    Map[y, x] = TerrainType.Floor;
                }
            }
        }
        for (int i = 5; i < 15; i++)
        {
            Map[i, 9] = TerrainType.Wall;
        }
        for (int i = 5; i < 15; i++)
        {
            Map[i, 30] = TerrainType.Wall;
        }
    }
    public void TryPickUpItem()
    {
        var items = EntityManager.GetItemsAt(EntityManager.GetEntityPosition(Player));
        if (items.Count() == 0)
        { return; }
        var topitem = items.First();
        EntityManager.RemoveEntity(topitem);
        Player.Inventory.AddToInventory(topitem);
        topitem.ApplyOnPickUp(Player);
    }
    public void TryThrowItem()
    {
        var item = Player.Inventory.GetSelectedItem();
        if (item == null)
        {
            return;
        }
        EntityManager.AddEntity(item, EntityManager.GetEntityPosition(Player));
        Player.Inventory.RemoveFromInventory(item);
        item.ApplyOnDePickUp(Player);
    }
    public void EquipRight()
    {
        var item = Player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (Player.Hands.RightEquip(item))
            {
                Player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(Player);
                return;
            }
        }
        var iteme = Player.Hands.RightUnequip();
        if (iteme != null)
        {
            Player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(Player);
        }
    }
    public void EquipLeft()
    {
        var item = Player.Inventory.GetSelectedItem();
        if (item != null)
        {
            if (Player.Hands.LeftEquip(item))
            {
                Player.Inventory.RemoveFromInventory(item);
                item.ApplyOnHanded(Player);
                return;
            }
        }
        var iteme = Player.Hands.LeftUnequip();
        if (iteme != null)
        {
            Player.Inventory.AddToInventory(iteme);
            iteme.ApplyOnDeHanded(Player);
        }

    }

}