using SmartWay.Orm.Entity;

namespace Contest.Domain.Games
{
    public interface IEliminationStepSetting : IStepSetting, IDistinctableEntity
    {
        /// <summary>
        ///     Get first elimination step
        /// </summary>
        EliminationType FirstStep { get; set; }
    }
}