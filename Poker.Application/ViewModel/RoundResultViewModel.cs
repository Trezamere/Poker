using System;
using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Poker.Application.Model;
using Poker.Core;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// ViewModel for a result of poker.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IRoundResultViewModel), ServiceLocatorRegistrationMode.Transient)]
    public class RoundResultViewModel : ViewModelBase, IRoundResultViewModel
    {
        #region Result property

        /// <summary>
        /// Identifies the <see cref="Result"/> property.
        /// </summary>
        public static readonly PropertyData ResultProperty = RegisterProperty("Result", typeof (IRoundResult));

        /// <summary>
        /// Gets or sets the player configuration.
        /// </summary>
        // TODO: Some sort of serialization issue between IHand and the IRoundResult, just set the properties by hand...
        //[Model]
        public IRoundResult Result
        {
            get { return GetValue<IRoundResult>(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        #endregion

        #region Winners property

        /// <summary>
        /// Identifies the <see cref="Winners"/> property.
        /// </summary>
        public static readonly PropertyData WinnersProperty = RegisterProperty("Winners",
            typeof (ObservableCollection<IPlayer>));

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        public ObservableCollection<IPlayer> Winners
        {
            get { return GetValue<ObservableCollection<IPlayer>>(WinnersProperty); }
            set { SetValue(WinnersProperty, value); }
        }

        #endregion

        #region Losers property

        /// <summary>
        /// Identifies the <see cref="Losers"/> property.
        /// </summary>
        public static readonly PropertyData LosersProperty = RegisterProperty("Losers",
            typeof(ObservableCollection<IPlayer>));

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        public ObservableCollection<IPlayer> Losers
        {
            get { return GetValue<ObservableCollection<IPlayer>>(LosersProperty); }
            set { SetValue(LosersProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundResultViewModel"/> class.
        /// </summary>
        public RoundResultViewModel(IRoundResult roundResult)
        {
            Argument.IsNotNull(() => roundResult);

            Result = roundResult;

            Winners = Result.Winners;
            Losers = Result.Losers;
        }

        #region Overrides of ViewModelBase

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                if (Winners.Count == 1)
                    return $"{Winners[0].Name} won!";

                if (Winners.Count > 1)
                    return "Tie!";

                return "Results...";
            }
            protected set { base.Title = value; }
        }

        #endregion
    }
}