namespace Contest.Core.Component
{
    public interface IComposer
    {
        /// <summary>
        /// Creates composable parts from an attributed objects and composes
        /// </summary>
        /// <param name="obj">attributed objects to compose.</param>
        void ComposeParts(object obj);
    }
}
