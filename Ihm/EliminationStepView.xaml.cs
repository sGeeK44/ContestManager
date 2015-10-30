using Contest.Business;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for Contest.xaml
    /// </summary>
    public partial class EliminationStepView
    {
        public EliminationStepView(EliminationStep step, bool isFirstEliminationStep, bool isLastEliminationStep)
        {
            InitializeComponent();
            DataContext = new EliminationStepViewVm(step, isFirstEliminationStep, isLastEliminationStep);
        }
    }
}
