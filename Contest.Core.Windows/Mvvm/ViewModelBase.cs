using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Contest.Core.Windows.Mvvm
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return;
            field = value;
            if (propertyName != null) RaisedPropertyChanged(propertyName);
        }

        protected void Set<T>(string propertyName, ref T field, T value)
        {
            if (Equals(field, value)) return;
            field = value;
            if (propertyName != null) RaisedPropertyChanged(propertyName);
        }

        public bool ThrowOnInvalidPropertyName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisedPropertyChanged(GetPropertyName(propertyExpression));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            RaisedPropertyChanged(propertyName);
        }

        protected void RaisedPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var prop = PropertyChanged;
            if (prop != null) prop(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;

            var msg = string.Format("Invalid property name. propertyName:{0}.", propertyName);

            if (ThrowOnInvalidPropertyName) throw new Exception(msg);
            Debug.Fail(msg);
        }

        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }
            var property = body.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return property.Name;
        }

        public void Dispose()
        {
            
        }
    }
}
