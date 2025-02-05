﻿//------------------------------------------------------------------------------
// Copyright 2017 Yassine Riahi and Liam Flookes. 
// Provided under a MIT License, see license file on github.
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
#if FBM_LEGACY
using FASTBuildMonitorVSIX.Manifests.Legacy;
#else
using FASTBuildMonitorVSIX.Manifests;
#endif
using Microsoft.VisualStudio.Shell;

namespace FASTBuildMonitorVSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FASTBuildMonitorPane))]
    [Guid(PackageGuids.guidFASTBuildMonitorPackageString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class FASTBuildMonitorPackage : Package
    {
        public DTE2 _dte;

        public static FASTBuildMonitorPackage _instance = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FASTBuildMonitor"/> class.
        /// </summary>
        public FASTBuildMonitorPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

#region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            FASTBuildMonitorCommand.Initialize(this);
            base.Initialize();

            _instance = this;

            ThreadHelper.ThrowIfNotOnUIThread();
            _dte = (DTE2)base.GetService(typeof(DTE));
        }

        public class VSIXPackageInformation
        {
            public string _version;
            public string _packageName;
            public string _authors;
        }

        public static VSIXPackageInformation GetCurrentVSIXPackageInformation()
        {
            VSIXPackageInformation outInfo = null;

            try
            {
                outInfo = new VSIXPackageInformation
                {
                    _version = Vsix.Version,
                    _authors = Vsix.Author,
                    _packageName = Vsix.Name,
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            return outInfo;
        }
    }

#endregion
}
