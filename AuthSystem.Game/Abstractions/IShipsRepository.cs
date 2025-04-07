using AuthSystem.DataModel.Game.Abstractions;

namespace AuthSystem.Game.Abstractions
{
    public interface IShipsRepository
    {
        IEnumerable<Ship> GetShips(); 
    }
}
