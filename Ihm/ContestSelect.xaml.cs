namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for ContestSelect.xaml
    /// </summary>
    public partial class ContestSelect
    {
        public ContestSelect()
        {
            InitializeComponent();
            var viewModel = new ContestSelectVm();
            DataContext = viewModel;
            viewModel.RequestClose += o => Close();
        }
    }
}
