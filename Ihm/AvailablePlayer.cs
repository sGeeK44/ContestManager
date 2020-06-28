using System;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Players;

namespace Contest.Ihm
{
    public class AvailablePlayer : ViewModelBase, IViewModelItem<Person>
    {
        private bool _isSelected;
        private Guid _guid;
        private string _lastName;
        private string _firstName;
        private string _alias;

        public AvailablePlayer(Person person)
        {
            FromT(person);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        public Guid Guid
        {
            get => _guid;
            set => Set(ref _guid, value);
        }

        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public string Alias
        {
            get => _alias;
            set => Set(ref _alias, value);
        }

        public void FromT(Person obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Guid = obj.Id;
            LastName = obj.LastName;
            FirstName = obj.FirstName;
            Alias = obj.Alias;
        }

        public Person ToT()
        {
            throw new NotImplementedException();
            //return Person.Create(LastName, FirstName, Alias);
        }

        public void UpdateT(Person obj)
        {
            obj?.SetIndentity(LastName, FirstName, Alias);
        }
    }
}