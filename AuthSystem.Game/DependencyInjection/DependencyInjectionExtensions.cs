using Microsoft.Extensions.DependencyInjection;
using AuthSystem.Game.Abstractions;
using AuthSystem.Game.Services;
using AspNetCoreInjection.TypedFactories;
using AuthSystem.DataModel.Game;
using AuthSystem.Game.Models;
using AuthSystem.Game.Repositories;

namespace AuthSystem.Game.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAuthSystemGame<Tinterface>(this IServiceCollection services) 
            where Tinterface : class, IGameInterface
        {
            services.AddSingleton<IPlayersManager, PlayersManager>();
            services.AddSingleton<ISessionsManager, SessionsManager>();
            services.RegisterTypedFactory<IBattlefieldFactory>().ForConcreteType<Battlefield>();
            services.AddTransient<IGameInterface, Tinterface>();
            services.RegisterTypedFactory<ISessionFactory>().ForConcreteType<Session>();
            services.AddTransient<ILevelsRepository, LevelsRepository>();
            services.AddTransient<IShipsRepository, ShipsRepository>();
            services.AddTransient<PlayersManagerFactory>(provider => 
            () => provider.GetRequiredService<IPlayersManager>());

            return services;
        }
    }
}
