using Poker.Application.ViewModel;

namespace Poker.Application.View
{
    public partial class PlayerConfigurationView
    {
        public PlayerConfigurationView(IPlayerConfigurationViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
