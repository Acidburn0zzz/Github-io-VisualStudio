﻿using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace GitHub.VisualStudio.Base
{
    public abstract class PackageBase : Package
    {
        IServiceProvider serviceProvider;
        protected IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
            set { serviceProvider = value; }
        }

        protected PackageBase()
        {
            ServiceProvider = this;
        }

        protected PackageBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected void AddTopLevelMenuItem(
            Guid guid,
            int cmdId,
            EventHandler eventHandler)
        {
            var mcs = GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            Debug.Assert(mcs != null, "No IMenuCommandService? Something is wonky");
            if (mcs == null)
                return;
            var id = new CommandID(guid, cmdId);
            var item = new MenuCommand(eventHandler, id);
            mcs.AddCommand(item);
        }

        protected void AddDynamicMenuItem(
            Guid guid,
            int cmdId,
            Func<bool> canEnable,
            Action execute)
        {
            var mcs = GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            Debug.Assert(mcs != null, "No IMenuCommandService? Something is wonky");
            if (mcs == null)
                return;
            var id = new CommandID(guid, cmdId);
            var item = new OleMenuCommand(
                (s, e) => execute(),
                (s, e) => { },
                (s, e) =>
                {
                    ((OleMenuCommand)s).Visible = canEnable();
                },
                id);
            mcs.AddCommand(item);
        }

        public T GetService<T>()
        {
            Debug.Assert(this.serviceProvider != null, "GetService<T> called before service provider is set");
            if (serviceProvider == null)
                return default(T);
            return (T)serviceProvider.GetService(typeof(T));
        }

        public T GetExportedValue<T>()
        {
            var componentModel = (IComponentModel)GetService<SComponentModel>();
            if (componentModel == null)
                return default(T);
            var exportProvider = componentModel.DefaultExportProvider;
            return exportProvider.GetExportedValue<T>();
        }
    }
}
