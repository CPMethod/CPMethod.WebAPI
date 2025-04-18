﻿using AuthSystem.DataModel;
using AuthSystem.DataModel.Game;
using AuthSystem.DataModel.Game.Abstractions;
using AuthSystem.Game.Abstractions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AuthSystem.Game.Models
{
    public class Session : IGameSession
    {
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IGameInterface _gameInterface;

        private Subject<IGameSession> _completed = new();
        public IObservable<IGameSession> Completed => _completed.AsObservable();

        public Level Level { get; private set; }

        public bool IsActive =>
            Oponent != null &&
            Battlefields.All(b => b!.IsReady && b!.Ships.Any(ship => !ship.IsDestroyed));

        public int BattlefieldSize { get; private set; }
        public IBattlefield?[] Battlefields { get; private set; }

        public User? Next { get; private set; }

        public User Creator { get; private set; }
        public User? Oponent { get; private set; }

        public List<string> InvitedPlayersIDs { get; private set; }

        public Session(
            User creator, 
            Level level,
            IBattlefieldFactory battlefieldFactory,
            IGameInterface gameInterface)
        {
            Level = level;
            Creator = creator;
            BattlefieldSize = level.BattlefieldSize;
            _battlefieldFactory = battlefieldFactory;
            _gameInterface = gameInterface;

            InvitedPlayersIDs = new List<string>();

            Battlefields = new IBattlefield?[2] 
            { 
                CreateBattlefield(creator, BattlefieldSize), 
                null 
            };

            Next = creator;
        }

        public void AddOponent(User oponent)
        {
            if (IsActive)
                return;

            Oponent = oponent;
            Battlefields[1] = CreateBattlefield(oponent, BattlefieldSize);

            _gameInterface.StartDeployment(this);
        }

        public IBattlefield? GetBattlefield(User player)
            => GetBattlefield(player.Id);

        public IBattlefield? GetBattlefield(string playerId)
            => Battlefields.FirstOrDefault(b 
                => b is not null && 
                   b.Owner is not null && 
                   b.Owner.Id == playerId);

        public IBattlefield? GetOponentBattlefield(User player)
            => GetOponentBattlefield(player.Id);

        public IBattlefield? GetOponentBattlefield(string playerId) 
            => Battlefields.FirstOrDefault(
                b => (Creator.Id == playerId || (Oponent is not null && Oponent.Id == playerId)) && 
                     b is not null && 
                     b.Owner is not null &&
                     b.Owner.Id != playerId); 

        #region private helpers

        private IBattlefield CreateBattlefield(User player, int size)
        {
            IBattlefield battlefield = _battlefieldFactory.Create(Level);
            battlefield.Owner = player;

            battlefield.StatusChanged.Take(1)
                                     .Subscribe(status => OnBattlefieldStatusChanged(status, battlefield));

            battlefield.GotHit.Subscribe(
                    coordinates => OnBattlefieldHit(coordinates, battlefield), 
                    () => OnBattlefieldDestroyed(battlefield));

            return battlefield;
        }

        private void OnBattlefieldHit(
            (int x, int y) coordinates, 
            IBattlefield battlefield)
        {
            _gameInterface.GotHit(this, battlefield.Owner!, coordinates);
            Next = battlefield.Owner;
        }

        private void OnBattlefieldStatusChanged(bool status, IBattlefield battlefield)
        {
            if (battlefield.Owner is null)
                return;

            IBattlefield? oponentBattlefield = GetOponentBattlefield(battlefield.Owner);

            if (oponentBattlefield is null)
                return;

            if (oponentBattlefield.IsReady && status)
            {
                _gameInterface.FinishDeployment(this);
                _gameInterface.StartGame(this);
            }
        }

        private void OnBattlefieldDestroyed(IBattlefield battlefield)
        {
            _completed.OnNext(this);
            _completed.OnCompleted();
            Next = null;

            _gameInterface.EndGame(this);
        }

        #endregion
    }
}
