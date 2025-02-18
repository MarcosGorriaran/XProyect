using System;

namespace ProyectXAPI.Models
{
    public class Profile : Model
    {
        #region Attributes
        const int ProfileNameLength = 50;
        private int ?_id;
        private string _profileName;
        private Acount _creator;
        #endregion

        #region Getters and Setters
        public virtual int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public virtual string ProfileName
        {
            get
            {
                return _profileName;
            }
            set
            {
                if(value == null) value = string.Empty;
                if (value.Length > ProfileNameLength)
                {
                    throw new Exception("Profile name is too long");
                }
                _profileName = value;
            }
        }
        public virtual Acount Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
            }
        }
        #endregion
        public override bool Equals(object? obj)
        {
            if(obj == null || !(obj is Profile))
            {
                return false;
            }
            Profile other = (Profile)obj;
            return other.Id == Id && Creator.Equals(other.Creator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Creator != null ? Creator.GetHashCode() : 0);
                hashCode = hashCode ^ (Id != null ? Id.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
