﻿using System;
using System.Diagnostics;
using GitHub.Services;
using Microsoft.TeamFoundation.Controls;

namespace GitHub.Extensions
{
    public static class VSExtensions
    {
        public static T TryGetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.TryGetService(typeof(T)) as T;
        }

        public static object TryGetService(this IServiceProvider serviceProvider, Type type)
        {
            var ui = serviceProvider as IUIProvider;
            if (ui != null)
                return ui.TryGetService(type);
            else
            {
                try
                {
                    return serviceProvider.GetService(type);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                }
                return null;
            }
        }

        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

        public static T GetExportedValue<T>(this IServiceProvider serviceProvider)
        {
            var ui = serviceProvider as IUIProvider;
            return ui != null
                ? ui.GetService<T>()
                : VisualStudio.Services.ComponentModel.DefaultExportProvider.GetExportedValue<T>();
        }

        public static ITeamExplorerSection GetSection(this IServiceProvider serviceProvider, Guid section)
        {
            return serviceProvider?.GetService<ITeamExplorerPage>()?.GetSection(section);
        }
    }
}
