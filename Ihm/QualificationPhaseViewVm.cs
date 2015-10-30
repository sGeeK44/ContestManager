using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class QualificationPhaseViewVm : ViewModel
    {
        public QualificationPhaseViewVm(IList<IGameStep> gameStepList)
        {
            QualificationStepList = new ObservableCollection<QualificationStepViewVm>(gameStepList.Select(item => new QualificationStepViewVm(item as QualificationStep)));
        }

        public ObservableCollection<QualificationStepViewVm> QualificationStepList { get; set; }
    }
}
