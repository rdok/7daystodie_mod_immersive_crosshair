using System;
using System.Collections.Generic;

namespace ImmersiveCrosshair.Harmony
{
    public static class Services
    {
        private static Dictionary<Type, Delegate> _database;

        private static void Add<TService>(Func<object[], TService> provider) =>
            _database[typeof(TService)] = provider;

        public static TService Get<TService>(params object[] args)
        {
            if (_database == null)
            {
                throw new Exception("Services not initialized");
            }

            if (_database.TryGetValue(typeof(TService), out var provider))
            {
                return ((Func<object[], TService>)provider)(args);
            }

            throw new Exception($"Service {typeof(TService)} not registered");
        }

        public static void Initialise()
        {
            if (_database != null) return;

            _database = new Dictionary<Type, Delegate>();
            var logger = new Logger();
            var settings = new Settings(logger);

            Add<ILogger>(args => logger);
            Add<IGuiDrawCrosshair>(args => new GuiDrawCrosshair(logger, settings));
            Add<ISettings>(args => settings);
        }
    }
}