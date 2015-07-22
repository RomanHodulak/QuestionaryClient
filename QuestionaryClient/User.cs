using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public User()
        {
        }

        public User(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }
    }
}
