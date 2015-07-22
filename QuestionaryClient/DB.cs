using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace QuestionaryClient
{
    public static class DB
    {
        public static SqlConnection Conntection { get { return con; } }
        static SqlConnection con;
        public static string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "lsd.spsejecna.net";
                builder.UserID = "hodular";
                builder.Password = "databaze";
                builder.InitialCatalog = "hodular";
                return builder.ConnectionString;
            }
        }
        public static string ConnectionStringLocal
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"localhost\SQLEXPRESS";
                builder.InitialCatalog = "questionary";
                builder.IntegratedSecurity = true;
                return builder.ConnectionString;
            }
        }

        public static string InitializeConnection()
        {
            string ret = "Connection initialized";
            if (con != null)
                ret = "Connection reinitialized";
            CloseConnection();
            con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();
                return ret;
            }
            catch (Exception e)
            {
                if (con != null)
                {
                    con.Close();
                    con = null;
                }
                return e.ToString();
            }
        }

        public static string CloseConnection()
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
            else
                return "Connection is not initialized";
            return "Connection terminated";
        }

        public static string LoadQuizes(out Quiz[] items)
        {
            return LoadQuizes(out items, 0);
        }

        public static string LoadQuizes(out Quiz[] items, int count)
        {
            List<Quiz> itemsList = new List<Quiz>();
            items = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = null;
                if (count < 1)
                    command2 = new SqlCommand("Select * from Quiz", con);
                else
                    command2 = new SqlCommand("Select TOP " + count + " * from Quiz", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    Quiz item = new Quiz();
                    item.ID = (int)sdr1["Quiz_ID"];
                    item.Description = sdr1["Description"].ToString();
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            items = itemsList.ToArray();
            if (items.Length != 1)
                return items.Length + " records loaded";
            else
                return items.Length + " record loaded";
        }

        public static string LoadQuestions(int quiz_ID, out Question[] questions)
        {
            List<Question> itemsList = new List<Question>();
            questions = null;
            int count = 0;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = null;
                if (count < 1)
                    command2 = new SqlCommand("Select * from Question", con);
                else
                    command2 = new SqlCommand("Select TOP " + count + " * from Question", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    Question item = new Question();
                    item.ID = (int)sdr1["Question_ID"];
                    item.Description = sdr1["Description"].ToString();
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            questions = itemsList.ToArray();
            if (questions.Length != 1)
                return questions.Length + " records loaded";
            else
                return questions.Length + " record loaded";
        }

        public static string LoadAnswers(int question_ID, out Answer[] items)
        {
            List<Answer> itemsList = new List<Answer>();
            items = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from Answer where Answer.Question_ID=" + question_ID + ";", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    Answer item = new Answer();
                    item.ID = (int)sdr1["Answer_ID"];
                    item.Question_ID = (int)sdr1["Question_ID"];
                    NumberFormatInfo info = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                    info.CurrencyDecimalSeparator = ".";
                    item.Correctness = (float)float.Parse(sdr1["Correctness"].ToString(), info);
                    item.Description = sdr1["Description"].ToString();
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            items = itemsList.ToArray();
            if (items.Length != 1)
                return items.Length + " records loaded";
            else
                return items.Length + " record loaded";
        }

        public static string LoadAnswers(int answer_id, out Answer item)
        {
            item = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from Answer where Answer.Answer_id=" + answer_id + ";", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    item = new Answer();
                    item.ID = (int)sdr1["Answer_ID"];
                    item.Question_ID = (int)sdr1["Question_ID"];
                    NumberFormatInfo info = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                    info.CurrencyDecimalSeparator = ".";
                    item.Correctness = (float)float.Parse(sdr1["Correctness"].ToString(), info);
                    item.Description = sdr1["Description"].ToString();
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            if (item == null)
            {
                return "0 record loaded";
            }
            return "1 record loaded";
        }

        public static string LoadUser(int user_ID, out User user)
        {
            user = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from UserTBL where UserTBL.User_ID=" + user_ID + ";", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    user = new User();
                    user.ID = (int)sdr1["User_ID"];
                    user.Name = sdr1["Name"].ToString();
                    user.Surname = sdr1["Surname"].ToString();
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "1 record loaded";
        }

        public static string LoadUser(string name, string surname, out User user)
        {
            user = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from UserTBL where UserTBL.Name='" + name + "' AND UserTBL.Surname='" + surname + "';", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    user = new User();
                    user.ID = (int)sdr1["User_ID"];
                    user.Name = sdr1["Name"].ToString();
                    user.Surname = sdr1["Surname"].ToString();
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            if (user == null)
            {
                return "0 record loaded";
            }
            return "1 record loaded";
        }

        public static string LoadResult(int quiz_id, int user_id, out Results user)
        {
            user = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from Results where Results.Quiz_ID='" + quiz_id + "' AND Results.User_ID='" + user_id + "';", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    user = new Results();
                    user.ID = (int)sdr1["Results_ID"];
                    user.Quiz_ID = quiz_id;
                    user.User_ID = user_id;
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            if (user == null)
            {
                return "0 record loaded";
            }
            return "1 record loaded";
        }

        public static string LoadQuestionResults(int results_id, out QuestionResult[] items)
        {
            List<QuestionResult> itemsList = new List<QuestionResult>();
            items = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from QuestionResult where QuestionResult.Results_id='" + results_id + "';", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    QuestionResult item = new QuestionResult();
                    item.ID = (int)sdr1["QuestionResult_ID"];
                    item.Answer_ID = (int)sdr1["Answer_ID"];
                    item.Results_ID = results_id;
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            items = itemsList.ToArray();
            if (items.Length != 1)
                return items.Length + " records loaded";
            else
                return items.Length + " record loaded";
        }

        public static string LoadQuestionResultsByAnswer(int answer_id, out QuestionResult[] items)
        {
            List<QuestionResult> itemsList = new List<QuestionResult>();
            items = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from QuestionResult where QuestionResult.Answer_id='" + answer_id + "';", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    QuestionResult item = new QuestionResult();
                    item.ID = (int)sdr1["QuestionResult_ID"];
                    item.Answer_ID = (int)sdr1["Answer_ID"];
                    item.Results_ID = (int)sdr1["Results_ID"];
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            items = itemsList.ToArray();
            if (items.Length != 1)
                return items.Length + " records loaded";
            else
                return items.Length + " record loaded";
        }

        public static string InsertResults(ref Results item)
        {
            SqlCommand command = new SqlCommand("Insert Into Results (Quiz_ID, User_ID) values(" + item.Quiz_ID + ", " + item.User_ID
                + ");", con);
            string s = ExecuteCommand(command);

            SqlCommand command2 = new SqlCommand("Select * from Results where Results.Quiz_ID='" + item.Quiz_ID + "' AND Results.User_ID='" + item.User_ID + "';", con);
            SqlDataReader sdr1 = command2.ExecuteReader();
            while (sdr1.Read())
            {
                item.ID = (int)sdr1["Results_ID"];
            }
            sdr1.Close();

            return s;
        }

        public static string InsertUser(ref User item)
        {
            SqlCommand command = new SqlCommand("Insert Into UserTBL (Name, Surname) values('" + item.Name + "', '" + item.Surname
                + "');", con);
            string s = ExecuteCommand(command);

            SqlCommand command2 = new SqlCommand("Select * from UserTBL where UserTBL.Name='" + item.Name + "' AND UserTBL.Surname='" + item.Surname + "';", con);
            SqlDataReader sdr1 = command2.ExecuteReader();
            while (sdr1.Read())
            {
                item.ID = (int)sdr1["User_ID"];
            }
            sdr1.Close();

            return s;
        }

        public static string InsertQuestionResults(QuestionResult item)
        {
            SqlCommand command = new SqlCommand("Insert Into QuestionResult (Results_ID, Answer_ID) values(" + item.Results_ID + ", " + item.Answer_ID
                + ");", con);
            string s = ExecuteCommand(command);

            SqlCommand command2 = new SqlCommand("Select * from QuestionResult where QuestionResult.Results_ID='" + item.Results_ID + "' AND QuestionResult.Answer_ID='" + item.Answer_ID + "';", con);
            SqlDataReader sdr1 = command2.ExecuteReader();
            while (sdr1.Read())
            {
                item.ID = (int)sdr1["QuestionResult_ID"];
            }
            sdr1.Close();

            return s;
        }

        /*
        public static string UpdateRecord(Ucet item)
        {
            SqlCommand command = new SqlCommand("Update Ucet Set cislo_uctu ='" + item.cisloUctu + "', castka_Kc = '" + item.castkaKc +
                "', typ_uctu = '" + item.typUctu + "' where id_ucet = " + item.ID + ";", con);
            return ExecuteCommand(command);
        }

        public static string LoadRecord(int id, out Ucet item)
        {
            List<Ucet> itemsList = new List<Ucet>();
            item = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = new SqlCommand("Select * from Ucet where id_ucet='" + id + "';", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    item = new Ucet();
                    item.ID = (int)sdr1["id_ucet"];
                    item.cisloUctu = sdr1["cislo_uctu"].ToString();
                    item.castkaKc = (int)sdr1["castka_Kc"];
                    item.typUctu = sdr1["typ_uctu"].ToString();
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            if (item != null)
                return "Record loaded";
            else
                return "Record " + id + " not found";
        }

        public static string LoadRecords(out Ucet[] items)
        {
            return LoadRecords(out items, 0);
        }

        public static string LoadRecords(out Ucet[] items, int count)
        {
            List<Ucet> itemsList = new List<Ucet>();
            items = null;
            if (con == null)
                return "Connection is not initialized";
            try
            {
                SqlCommand command2 = null;
                if (count < 1)
                    command2 = new SqlCommand("Select * from Ucet", con);
                else
                    command2 = new SqlCommand("Select TOP " + count + " * from Ucet", con);
                SqlDataReader sdr1 = command2.ExecuteReader();
                while (sdr1.Read())
                {
                    Ucet item = new Ucet();
                    item.ID = (int)sdr1["id_ucet"];
                    item.typUctu = sdr1["typ_uctu"].ToString();
                    item.castkaKc = (int)sdr1["castka_Kc"];
                    item.cisloUctu = sdr1["cislo_uctu"].ToString();
                    itemsList.Add(item);
                }
                sdr1.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            items = itemsList.ToArray();
            if (items.Length != 1)
                return items.Length + " records loaded";
            else
                return items.Length + " record loaded";
        }
        */
        public static string ExecuteCommand(SqlCommand cmd)
        {
            try
            {
                int rows2 = cmd.ExecuteNonQuery();
                return "Rows affected: " + rows2;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string ExecuteCommand(SqlCommand cmd, string msg)
        {
            try
            {
                int rows2 = cmd.ExecuteNonQuery();
                return msg;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string ToSQLDate(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }

        public static DateTime ToDate(string sqldate)
        {
            int i1 = sqldate.IndexOf('-');
            int i2 = sqldate.LastIndexOf('-');
            string s = sqldate.Substring(0, i1);
            string s1 = sqldate.Substring(i1 + 1, i2 - i1 - 1);
            string s2 = sqldate.Substring(i2 + 1);
            return new DateTime(int.Parse(sqldate.Substring(0, i1)),
                int.Parse(sqldate.Substring(i1 + 1, i2 - i1 - 1)),
                int.Parse(sqldate.Substring(i2 + 1)));
        }

        static DateTime ToReaderDate(string sqldate)
        {
            string s = sqldate.Substring(0, sqldate.IndexOf(' '));
            int i1 = s.IndexOf('.');
            int i2 = s.LastIndexOf('.');
            return new DateTime(int.Parse(s.Substring(i2 + 1)), int.Parse(s.Substring(i1 + 1, i2 - i1 - 1)), int.Parse(s.Substring(0, i1)));
        }
    }
}
