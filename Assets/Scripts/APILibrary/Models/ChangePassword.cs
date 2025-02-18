using System;

namespace ProyectXAPI.Models
{
    public class ChangePassword : Acount
    {
        private string _newPassword;

        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                if (value.Length > MaxPasswordLength)
                {
                    throw new Exception("Password is too long");
                }
                _newPassword = value;
            }
        }
    }
}
