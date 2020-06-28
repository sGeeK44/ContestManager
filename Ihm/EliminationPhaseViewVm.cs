using System;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;

namespace Contest.Ihm
{
    public class EliminationPhaseViewVm : ViewModel
    {
        public EliminationPhaseViewVm(IPhase phase)
        {
            if (!(phase.GameStepList.First() is IEliminationStep firsStep)) throw  new NotSupportedException();
            EliminationStepList = new ObservableCollection<EliminationStepView>();
            var countGameStep = EliminationStep.CountStep(firsStep.Type);
            for (var i = 0; i < countGameStep; i++)
            {
                EliminationStepList.Add(null);
            }
            for (var i = 0; i < phase.GameStepList.Count; i++)
            {
                var temp = phase.GameStepList[i] as EliminationStep;
                EliminationStepList[EliminationStep.IndexStep(temp.Type)] = new EliminationStepView(temp, i == 0, i + 1 == countGameStep);
            }

            phase.NextStepStarted += (sender, step) =>
            {
                var temp = step as EliminationStep;
                var i = EliminationStep.IndexStep(temp.Type);
                EliminationStepList[i] = new EliminationStepView(temp, i + 1 == countGameStep, i == 0);
            };
        }

        public ObservableCollection<EliminationStepView> EliminationStepList { get; set; }
    }
}
