namespace Contest.Domain.Games
{
    public interface IEliminationStepSetting : IStepSetting
    {
        /// <summary>
        ///     Get first elimination step
        /// </summary>
        EliminationType FirstStep { get; set; }
    }
}