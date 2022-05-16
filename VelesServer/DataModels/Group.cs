using System.Collections.Generic;

namespace VelesServer.DataModels
{
    public class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
        }
        //Primary key
        public int Id { get; set; }
        public string GroupName { get; set; }

        //Foreign key to the users table
        //public int UserId { get; set; }
        //Relation one to many between users and groups 
        //public Users Users { get; set; }

        //Relation many to many between users and groups 
        public virtual ICollection<User> Users { get; set; }

        //Relation one to many Messages and Groups
        //public ICollection<Messages> Messages { get; set; }


    
    }
}