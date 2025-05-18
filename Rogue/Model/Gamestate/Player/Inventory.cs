namespace Rogue;
public class Inventory
{
    private List<IItem> _inventory = new();
    private int _cursor = -1;
    public int Cursor
    {
        get => _cursor; private set
        {
            if (_inventory.Count == 0)
            {
                _cursor = -1;
            }
            else
            {
                _cursor = Math.Clamp(value, 0, _inventory.Count - 1);
            }
        }
    }
    public Inventory()
    {
    }
    public void AddToInventory(IItem item)
    {
        _inventory.Add(item);
    }
    public void RemoveFromInventory(IItem item)
    {
        _inventory.Remove(item);
        if (_inventory.Count == 0)
        {
            Cursor = -1;
        }
        else if (Cursor >= _inventory.Count)
        {
            Cursor = _inventory.Count - 1;
        }
    }
    public List<IItem> GetInventory()
    {
       return _inventory;
    }
    public int InventoryCount()
    {
        return _inventory.Count();
    }
    public void MoveCursor(int direction)
    {
        if (_inventory.Count == 0)
        {
            Cursor = -1;
            return;
        }
        int newCursor = Cursor + direction;
        Cursor = newCursor;
    }
    public IItem? GetSelectedItem()
    {
        if (Cursor == -1 || _inventory.Count == 0)
        { return null; }
        return _inventory[Cursor];
    }
}