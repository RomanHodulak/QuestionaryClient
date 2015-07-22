using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class Question
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Quiz_ID { get; set; }

        public Question()
        {
        }

        public Question(string description, int quiz_id)
        {
            this.Description = description;
            this.Quiz_ID = quiz_id;
        }
    }
}
