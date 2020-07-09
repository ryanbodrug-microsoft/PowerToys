﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;
using Windows.Foundation;

namespace Microsoft.Plugin.Program.Programs
{

    /// <summary>
    /// This is a simple wrapper class around the PackageCatalog to facilitate unit testing.
    /// </summary>
    internal class PackageCatalogWrapper : IPackageCatalog
    {
        PackageCatalog _packageCatalog;

        public PackageCatalogWrapper()
        {
            //Opens the catalog of packages that is available for the current user.
            _packageCatalog = PackageCatalog.OpenForCurrentUser();
        }

        //
        // Summary:
        //     Indicates that an app package is installing.
        public event TypedEventHandler<PackageCatalog, PackageInstallingEventArgs> PackageInstalling
        {
            add
            {
                _packageCatalog.PackageInstalling += value;
            }

            remove
            {
                _packageCatalog.PackageInstalling -= value;
            }
        }

        //
        // Summary:
        //     Indicates that an app package is uninstalling.
        public event TypedEventHandler<PackageCatalog, PackageUninstallingEventArgs> PackageUninstalling
        {
            add
            {
                _packageCatalog.PackageUninstalling += value;
            }

            remove
            {
                _packageCatalog.PackageUninstalling -= value;
            }
        }


        //
        // Summary:
        //     Indicates that an app package is updating.
        public event TypedEventHandler<PackageCatalog, PackageUpdatingEventArgs> PackageUpdating
        {
            add
            {
                _packageCatalog.PackageUpdating += value;
            }

            remove
            {
                _packageCatalog.PackageUpdating -= value;
            }
        }

    }
}
