

namespace ProyectXAPI.Models
{
    public class SessionData : Model
    {
        const int ProfileLength = 50;
        const int ProfileCreatorLength = 50;
        const int SessionLength = 10;

        private Profile _profile;
        private Session _session;
        private int _kills;
        private int _deaths;

        public virtual Profile Profile
        {
            get
            {
                return _profile;
            }
            set
            {
                _profile = value;
            }
        }
        public virtual Session Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }
        public virtual int Kills
        {
            get
            {
                return _kills;
            }
            set
            {
                _kills = value;
            }
        }
        public virtual int Deaths
        {
            get
            {
                return _deaths;
            }
            set
            {
                _deaths = value;
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is SessionData))
            {
                return false;
            }
            SessionData other = (SessionData)obj;
            return other.Profile == Profile && Session == other.Session;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Profile != null ? Profile.GetHashCode() : 0);
                hashCode = hashCode ^ (Session != null ? Session.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
