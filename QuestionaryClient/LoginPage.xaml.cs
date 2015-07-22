using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuestionaryClient
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        Storyboard errorAnim;
        Storyboard succAnim;
        public bool Wait { get { return wait; } }
        bool wait = false;
        public event RoutedEventHandler Submit;

        public LoginPage()
        {
            InitializeComponent();
            errorAnim = Resources["errorAnim"] as Storyboard;
            succAnim = Resources["succAnim"] as Storyboard;
            succAnim.Completed += succAnim_Completed;
        }

        void succAnim_Completed(object sender, EventArgs e)
        {
            wait = false;
            if (Submit != null)
                Submit.Invoke(this, new RoutedEventArgs());
        }

        public User login()
        {
            jmeno.IsEnabled = false;
            prijmeni.IsEnabled = false;
            User user;
            try
            {
                DB.LoadUser(jmeno.Text, prijmeni.Text, out user);
                if (user == null)
                {
                    if (jmeno.Text.Length < 1)
                    {
                        jmeno.IsEnabled = true;
                        prijmeni.IsEnabled = true;
                        errorMsg.Text = "Please enter your name";
                        errorMsg.Foreground = Brushes.Red;
                        errorAnim.Begin();
                        return null;
                    }
                    else if (prijmeni.Text.Length < 1)
                    {
                        jmeno.IsEnabled = true;
                        prijmeni.IsEnabled = true;
                        errorMsg.Text = "Please enter your surname";
                        errorMsg.Foreground = Brushes.Red;
                        errorAnim.Begin();
                        return null;
                    }
                    user = new User(jmeno.Text, prijmeni.Text);
                    DB.InsertUser(ref user);
                    errorMsg.Text = "Registered as a new user";
                    errorMsg.Foreground = Brushes.Green;
                    succAnim.Begin();
                    wait = true;
                    return user;
                }
                else
                {
                    return user;
                }
            }
            catch
            {
                jmeno.IsEnabled = true;
                prijmeni.IsEnabled = true;
                errorMsg.Text = "An error has occurred";
                errorMsg.Foreground = Brushes.Red;
                errorAnim.Begin();
                return null;
            }
            return null;
        }

        private void jmeno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (Submit != null)
                    Submit.Invoke(this, new RoutedEventArgs());
        }
    }
}
