using AuthSystem.DataModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Game.Abstractions
{
    public interface ILevelsRepository
    {
        IEnumerable<Level> GetLevels(); 
    }
}
