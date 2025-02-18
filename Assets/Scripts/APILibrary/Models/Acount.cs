using System;

namespace ProyectXAPI.Models
{
    public class Acount : Model
    {
        const int MaxUsernameLength = 50;
        protected const int MaxPasswordLength = 100;
        private string _username;
        private string _password;
        public virtual string Username 
        {
            get 
            {
                return _username;
            }
            set 
            {
                if(value.Length > MaxUsernameLength)
                {
                    throw new Exception("Username is too long");
                }
                _username = value; 
            }
        }
        public virtual string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if(value.Length > MaxPasswordLength)
                {
                    throw new Exception("Password is too long");
                }
                _password = value;
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Acount))
            {
                return false;
            }
            Acount other = (Acount)obj;
            return Username == other.Username;
        }
    }
}
