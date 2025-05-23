﻿using AuthSystem.DataModel.Game.Abstractions;
using AuthSystem.DataModel.Game.Ships;
using AuthSystem.Game.Abstractions;

namespace AuthSystem.Game.Repositories
{
    public class ShipsRepository : IShipsRepository
    {
        public IEnumerable<Ship> GetShips()
        {
            return new Ship[] 
            {
                // Corvettes
                new Corvette(100)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 20, DamageRadius = 1 }
                    }
                },

                new Corvette(100)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 20, DamageRadius = 1 }
                    }
                },

                new Corvette(100)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 20, DamageRadius = 1 }
                    }
                },

                new Corvette(100)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 20, DamageRadius = 1 }
                    }
                },

                // Frigates
                new Frigate(150)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 50, DamageRadius = 1 }
                    }
                },

                new Frigate(150)
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 50, DamageRadius = 1 }
                    }
                },

                // Destroyers
                new Destroyer(150) 
                {
                    Weapons = new[]
                    {
                        new Weapon { Damage = 50, DamageRadius = 2 }
                    }
                },

                //new Cruiser(200)
                //{
                //    Weapons = new[]
                //    {
                //        new Weapon { Damage = 150, DamageRadius = 2 },
                //        new Weapon { Damage = 150, DamageRadius = 2 }
                //    }
                //},

                //new Battleship(250)
                //{
                //    Weapons = new[]
                //    {
                //        new Weapon { Damage = 100, DamageRadius = 3 },
                //        new Weapon { Damage = 100, DamageRadius = 3 }
                //    }
                //}
            };
        }
    }
}
