

using System;

namespace ProyectXAPI.Models
{
    public class Session : Model
    {
        
        private int? _sessionID;
        private DateTime? _dateGame;
        public virtual int? SessionID
        {
            get
            {
                return _sessionID;
            }
            set
            {
                _sessionID = value;
            }
        }
        public virtual DateTime? DateGame
        {
            get
            {
                return _dateGame;
            }
            set
            {
                _dateGame = value;
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Session))
            {
                return false;
            }
            Session other = (Session)obj;
            return other.SessionID == SessionID;
        }
    }
}
