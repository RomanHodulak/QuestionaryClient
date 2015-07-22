using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class Quiz
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public Quiz()
        {
        }

        public Quiz(string description)
        {
            this.Description = description;
        }
    }
}
