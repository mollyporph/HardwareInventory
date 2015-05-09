using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HardwareInventory.Datamodel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region backingProps
        private string _validationErrorMessage;
        private string _username;
        private string _password;
        private bool _validationFailed;
        #endregion
        public string ValidationErrorMessage
        {
            get { return _validationErrorMessage; }
            set
            {
                _validationErrorMessage = value;
                OnPropertyChanged();
            }
        }
        public bool ValidationFailed
        {
            get { return _validationFailed; }
            set
            {
                _validationFailed = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
