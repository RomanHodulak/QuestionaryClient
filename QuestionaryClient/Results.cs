using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class Results
    {
        public int ID { get; set; }
        public int Quiz_ID { get; set; }
        public int User_ID { get; set; }

        public Results()
        {
        }

        public Results(int quiz_id, int user_id)
        {
            this.Quiz_ID = quiz_id;
            this.User_ID = user_id;
        }
    }
}
