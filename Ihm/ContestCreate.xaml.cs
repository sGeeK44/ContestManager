namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for ContestCreate.xaml
    /// </summary>
    public partial class ContestCreate
    {
        public ContestCreate()
        {
            InitializeComponent();
            var viewModel = new ContestCreateVm();
            DataContext = viewModel;
            viewModel.RequestClose += o => Close();
        }
    }
}
