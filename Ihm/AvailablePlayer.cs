using System;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

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
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        public Guid Guid
        {
            get { return _guid; }
            set { Set(ref _guid, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public string Alias
        {
            get { return _alias; }
            set { Set(ref _alias, value); }
        }

        public void FromT(Person obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
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
            if (obj == null) return;
            obj.SetIndentity(LastName, FirstName, Alias);
        }
    }
}