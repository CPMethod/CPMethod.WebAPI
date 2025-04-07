using AuthSystem.DataModel;
using AuthSystem.DataModel.Game;

namespace AuthSystem.Game.Abstractions
{
    public interface ISessionFactory
    {
        IGameSession Create(User creator, Level level);
    }
}
