using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class Answer
    {
        public int ID { get; set; }
        public int Question_ID { get; set; }
        public string Description { get; set; }
        public float Correctness { get; set; }

        public Answer()
        {
        }

        public Answer(int question_id, string description, float correctness)
        {
            this.Question_ID = question_id;
            this.Correctness = correctness;
            this.Description = description;
        }
    }
}
