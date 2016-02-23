using Contest.Core.Repository;

namespace Contest.Business
{
    public interface IEliminationStepSetting : IStepSetting
    {
        /// <summary>
        /// Get first elimination step
        /// </summary>
        EliminationType FirstStep { get; set; }
    }
}