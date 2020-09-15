using System.Collections.ObjectModel;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Players;
using Contest.Service;

namespace Contest.Ihm
{
    public class RegisterPlayerVm : ViewModel
    {
        #region Constructors

        public RegisterPlayerVm(IPerson personToUpdate = null)
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
                        var contestService = new ContestService();
                        if (Player == null)
                        {
                            contestService.CreatePerson(LastName, FirstName, Alias, Mail, CanMailing, IsMemberOfAssociation);
                        }
                        else
                        {
                            contestService.UpdatePerson(Player, LastName, FirstName, Alias, Mail, CanMailing, IsMemberOfAssociation);
                        }
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
