using System.Windows;
using Catel.IoC;
using Catel.Windows;

namespace Poker.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        #region Overrides of Application

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            Catel.Logging.LogManager.AddDebugListener();
#endif
            StyleHelper.CreateStyleForwardersForDefaultStyles();

            IServiceLocator serviceLocator = InitializeContainer();

            base.OnStartup(e);
        }

        /// <summary>
        /// Initializes the IoC container.
        /// </summary>
        private IServiceLocator InitializeContainer()
        {
            var serviceLocator = ServiceLocator.Default;
            serviceLocator.AutoRegisterTypesViaAttributes = true;

            new Core.IoC.Bootstrapper().Configure(serviceLocator);

            new IoC.Bootstrapper().Configure(serviceLocator);

            return serviceLocator;
        }

        #endregion
    }
}
