using System;
using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using EnhancePoE.App.Extensions;
using EnhancePoE.App.Helpers;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;
using EnhancePoE.UI.UserControls;
using EnhancePoE.UI.View;
using EnhancePoE.UI.ViewModels;

namespace EnhancePoE.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public class AppBootstrapper : BootstrapperBase
    {
        #region Fields
        
        private SimpleContainer _simpleContainer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Initialize();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            _simpleContainer = new SimpleContainer();

            _simpleContainer.PerRequest<ApplicationSettingService>();
            _simpleContainer.PerRequest<HotkeysManager>();
            
            _simpleContainer.PerRequest<ChaosRecipeEnhancerViewModel>();
            _simpleContainer.PerRequest<DynamicGridControlViewModel>();
            _simpleContainer.PerRequest<DynamicGridControlQuadViewModel>();
            _simpleContainer.PerRequest<MainOverlayBase>();
            _simpleContainer.PerRequest<MainOverlayContentViewModel>();
            _simpleContainer.PerRequest<MainOverlayContentMinifiedViewModel>();
            _simpleContainer.PerRequest<MainOverlayOnlyButtonsViewModel>();

            // _simpleContainer.RegisterInstance(typeof(SimpleContainer), null, _simpleContainer);
        }
        
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The located service.
        /// </returns>
        protected override object GetInstance(Type service, string key)
        {
            return _simpleContainer.GetInstance(service, key);
        }
        
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>
        /// The located services.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _simpleContainer.GetAllInstances(service);
        }
        
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }
        
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            var settings = _simpleContainer.GetInstance<ApplicationSettingService>();
            
            // this._sentry = SentrySdk.Init(Dsn);
            // SentrySdk.ConfigureScope((c) =>
            // {
            //     c.User = new User()
            //     {
            //         Id = settings.CREClientId,
            //     };
            // });

            if (RunningInstance() != null)
            {
                System.Windows.MessageBox.Show("Another Instance Is Running");
                System.Windows.Application.Current.Shutdown();
                return;
            }

            DisplayRootViewFor<MainWindowView>();
        }

        /// <summary>
        /// Runnings the instance.
        /// </summary>
        /// <returns>The other running instance.</returns>
        public static Process RunningInstance()
        {
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);

            try
            {
                var currentFilePath = currentProcess.GetMainModuleFileName();
                foreach (var process in processes)
                {
                    if (process.Id != currentProcess.Id)
                    {
                        if (process.GetMainModuleFileName() == currentFilePath)
                        {
                            return process;
                        }
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                AdminRequestHelper.RequestAdmin();
            }

            return null;
        }
        
        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            
            // Add real logging
            Console.WriteLine(exception);
            Console.WriteLine(exception.Message);
        }

        #endregion
    }
}