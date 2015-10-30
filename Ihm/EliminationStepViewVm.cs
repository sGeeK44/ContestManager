using System.Collections.Generic;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class EliminationStepViewVm : ViewModel
    {
        public EliminationStepViewVm(EliminationStep step, bool isFirstEliminationStep, bool isLastEliminationStep)
        {
            MatchList = new List<EliminationMatchConteneurVm>();
            foreach (var match in step.MatchList)
            {
                MatchList.Add(new EliminationMatchConteneurVm(match, isFirstEliminationStep, isLastEliminationStep));
            }
            Count = MatchList.Count;
        }

        public int Count { get; set; }
        public List<EliminationMatchConteneurVm> MatchList { get; set; }
    }
}
