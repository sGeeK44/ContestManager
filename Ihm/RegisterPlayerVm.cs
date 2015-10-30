using Contest.Business;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class RegisterPlayerVm : ViewModel
    {
        #region Constructors

        public RegisterPlayerVm(IPerson personToUpdate)
        {
            Player = personToUpdate;

            if (Player != null)
            {
                Title = "Modifier un joueur";
                LastName = Player.LastName;
                FirstName = Player.FirstName;
                Alias = Player.Alias;
                Mail = Player.Mail;
                CanMailing = Player.CanMailing;
                IsMemberOfAssociation = Player.IsMember;
            }
            else Title = "Enregister un joueur";

            Save = new RelayCommand(
                delegate
                    {
                        if (Player == null)
                        {
                            Player = Person.Create(LastName, FirstName, Alias);
                        }
                        else Player.SetIndentity(LastName, FirstName, Alias);
                        Player.Mail = Mail;
                        Player.CanMailing = CanMailing;
                        Player.IsMember = IsMemberOfAssociation;
                        CloseCommand.Execute(Player);
                    },
                delegate
                    {
                        return !string.IsNullOrEmpty(Alias);
                    });
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Alias { get; set; }
        public string Mail { get; set; }
        public bool CanMailing { get; set; }
        public bool IsMemberOfAssociation { get; set; }
        public IPerson Player { get; private set; }

        #endregion

        #region Commands

        public RelayCommand Save { get; set; }

        #endregion
    }
}
