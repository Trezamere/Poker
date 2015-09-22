using System;
using System.Collections.Generic;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Poker.Application.Model;
using Poker.Application.ViewModel;
using Poker.Application.View;
using Poker.Core;
using Poker.Core.Deck;
using Poker.Core.Hands;

namespace Poker.Application.IoC
{
    /// <summary>
    /// Bootstrapper for this assembly which is used to configure the IoC container.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Configures the specified IoC container with the type resolutions from this library.
        /// </summary>
        public void Configure(IServiceLocator container)
        {
            container.RegisterInstance<Func<IPlayerConfigurationViewModel>>(() => new PlayerConfigurationViewModel(container.ResolveType<IPlayerConfiguration>()));
            container.RegisterInstance<Func<IRoundResult, IRoundResultViewModel>>(result => new RoundResultViewModel(result));
            container.RegisterInstance<Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>, IRoundResult>>((winners, losers) => new RoundResult(winners, losers));

            var viewModelLocator = container.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof (MainWindow), typeof (MainWindowViewModel));

            var uiVisualizerService = container.ResolveType<IUIVisualizerService>();
            uiVisualizerService.Register(typeof(PlayerConfigurationViewModel), typeof(PlayerConfigurationView));
            uiVisualizerService.Register(typeof(RoundResultViewModel), typeof(RoundResultView));
        }
    }
}