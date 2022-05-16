using System.Collections.Generic;

namespace VelesServer.DataModels
{
    public class User
    {
        //Primary key
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        //Relation one to many between users and groups (owner)
        //public ICollection<Groups> Groups { get; set; }
        //Relation many to many between users and groups (participants)
        //public virtual ICollection<Groups> Group { get; set; }
        
    }
}
