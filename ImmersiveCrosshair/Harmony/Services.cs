using System;
using System.Collections.Generic;

namespace ImmersiveCrosshair.Harmony
{
    public static class Services
    {
        private static readonly Dictionary<Type, Delegate> Database =
            new Dictionary<Type, Delegate>();

        private static void Add<TService>(Func<object[], TService> provider)
        {
            Database[typeof(TService)] = provider;
        }

        public static TService Get<TService>(params object[] args)
        {
            if (Database.TryGetValue(typeof(TService), out var provider))
            {
                return ((Func<object[], TService>)provider)(args);
            }

            throw new Exception($"Service {typeof(TService)} not registered");
        }

        public static void Initialise()
        {
            var logger = new Logger();
            var settings = new Settings(logger);

            Add<ILogger>(args => new Logger());
            Add<IGuiDrawCrosshair>(args => new GuiDrawCrosshair(logger, settings));
            Add<ISettings>(args => new Settings(logger));
        }
    }
}