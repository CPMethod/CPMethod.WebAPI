using AuthSystem.DataModel.Game;
using AuthSystem.DataModel.Game.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Game.Abstractions
{
    public interface IBattlefieldFactory
    {
        IBattlefield Create(Level level);
    }
}
