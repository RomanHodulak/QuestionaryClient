using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionaryClient
{
    public class QuestionResult
    {
        public int ID { get; set; }
        public int Answer_ID { get; set; }
        public int Results_ID { get; set; }

        public QuestionResult()
        {
        }

        public QuestionResult(int answer_id, int results_id)
        {
            this.Results_ID = results_id;
            this.Answer_ID = answer_id;
        }
    }
}
