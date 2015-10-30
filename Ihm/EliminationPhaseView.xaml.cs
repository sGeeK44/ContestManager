using System;
using Contest.Business;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for Contest.xaml
    /// </summary>
    public partial class EliminationPhaseView
    {
        public EliminationPhaseView(IPhase phase)
        {
            if (phase == null) throw new ArgumentNullException("phase");

            InitializeComponent();
            DataContext = new EliminationPhaseViewVm(phase);
        }
    }
}
