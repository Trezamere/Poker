using Poker.Application.ViewModel;

namespace Poker.Application.View
{
    /// <summary>
    /// Interaction logic for RoundResultView.xaml
    /// </summary>
    public partial class RoundResultView
    {
        public RoundResultView(IRoundResultViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
