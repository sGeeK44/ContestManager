namespace Contest.Core.Component
{
    /// <summary>
    /// Custom composition container
    /// </summary>
    public class FlippingContainer
    {
        private static readonly FlippingContainer instance = new FlippingContainer();
        
        public IComposer Current { get; set; }

        private FlippingContainer()
        {
            Current = new ExecutingAssemblies();
        }

        /// <summary>
        /// Creates composable parts from an attributed objects and composes
        /// </summary>
        /// <param name="obj">attributed objects to compose.</param>
        public void ComposeParts(object obj)
        {
            if (Current == null) return;
            Current.ComposeParts(obj);
        }

        /// <summary>
        /// Provide access to Container
        /// </summary>
        public static FlippingContainer Instance
        {
            get { return instance; }
        }
    }
}
