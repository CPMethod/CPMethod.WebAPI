using AuthSystem.DataModel.Game;
using AuthSystem.DataModel.Game.Abstractions;
using AuthSystem.DataModel.Game.Ships;
using AuthSystem.Game.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Game.Repositories
{
    public class LevelsRepository : ILevelsRepository
    {
        public IEnumerable<Level> GetLevels()
        {
            return new Level[]
            {
                new Level
                {
                    BattlefieldSize = 8,
                    AvailableTypes = new Dictionary<Type, int>
                    {
                        { typeof(Corvette), 4 },
                        { typeof(Frigate), 1 }
                    }
                },

                new Level
                {
                    BattlefieldSize = 10,
                    AvailableTypes = new Dictionary<Type, int>
                    {
                        { typeof(Corvette), 4 },
                        { typeof(Frigate), 2 }
                    }
                },

                new Level
                {
                    BattlefieldSize = 12,
                    AvailableTypes = new Dictionary<Type, int>
                    {
                        { typeof(Corvette), 4 },
                        { typeof(Frigate), 2 },
                        { typeof(Destroyer), 1 }
                    }
                }
            };
        }
    }
}
