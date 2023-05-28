using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eurovizio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string constring = "datasource = 127.0.0.1; port=3306;username=root;password=qweasd;database=eurovizio";
        MySqlConnection con = new MySqlConnection();
        ObservableCollection<Performer> performers = new ObservableCollection<Performer>();
        public MainWindow()
        {
            InitializeComponent();
            DatabaseConnect();
            InitializeDataGrid();
            CloseDatabaseConnection();
        }

        public void DatabaseConnect()
        {
           if (con != null)
            {
                con = new MySqlConnection(constring);
            }
            con.Open();
        }

        public void InitializeDataGrid()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM dal", con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                performers.Add(new Performer(Int32.Parse(reader.GetValue(1).ToString()), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), Int32.Parse(reader.GetValue(5).ToString()), Int32.Parse(reader.GetValue(6).ToString())));
            }
            dgPerformers.ItemsSource = performers;
            dgPerformers.SelectedIndex = 0;
        }


        public void CloseDatabaseConnection()
        {
            con.Close();
            con.Dispose();
        }

        private void TaskFour(object sender, RoutedEventArgs e)
        {
            DatabaseConnect();
            int performerCount = 0;
            int highestPlace = 0;
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) from DAL where orszag = @orszag", con);
            command.Parameters.AddWithValue("@orszag", "Magyarország");
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                performerCount = Int32.Parse(reader.GetValue(0).ToString());
            }
            reader.Close();
            command = new MySqlCommand("SELECT helyezes from DAL where orszag = @orszag ORDER BY helyezes ASC LIMIT 1;", con);
            command.Parameters.AddWithValue("@orszag", "Magyarország");
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                highestPlace = Int32.Parse(reader.GetValue(0).ToString());
            }
            CloseDatabaseConnection();
            MessageBox.Show($"Magyarországi versenyzők száma: {performerCount}, legmagasabb helyezés: {highestPlace}");
        }

        private void TaskFive(object sender, RoutedEventArgs e)
        {
            DatabaseConnect();
            float averagePoints = 0;
            MySqlCommand command = new MySqlCommand("SELECT ROUND(AVG(pontszam), 2) from DAL where orszag = @orszag;", con);
            command.Parameters.AddWithValue("@orszag", "Németország");
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                averagePoints = float.Parse(reader.GetValue(0).ToString());
            }
            CloseDatabaseConnection();
            MessageBox.Show($"Németországi versenyzők átlagos pontszáma: {averagePoints}");
        }

        private void TaskSix(object sender, RoutedEventArgs e)
        {
            DatabaseConnect();
            List<string> songsThatContainTheWordLunk = new List<string>();
            MySqlCommand command = new MySqlCommand("SELECT eloado, cim from dal WHERE cim LIKE '%luck%';", con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                songsThatContainTheWordLunk.Add(reader.GetValue(0).ToString() + " - " + reader.GetValue(1).ToString());
            }
            CloseDatabaseConnection();
            string msgBoxString = "";
            foreach (string s in songsThatContainTheWordLunk)
            {
                msgBoxString += s + "\n";
            }
            MessageBox.Show(msgBoxString);
        }

        private void TaskSeven(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> searchResults = new ObservableCollection<string>();

            DatabaseConnect();
            MySqlCommand command = new MySqlCommand("SELECT cim from dal WHERE eloado LIKE @input ORDER BY eloado ASC, cim ASC;", con);
            command.Parameters.AddWithValue("@input", "%" + tbPerformer.Text + "%");
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                searchResults.Add(reader.GetValue(0).ToString());
            }
            CloseDatabaseConnection();
            lbPerformer.ItemsSource = searchResults;
        }

        private void dgSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DatabaseConnect();
            MySqlCommand command = new MySqlCommand("SELECT datum FROM verseny WHERE ev = @ev", con);
            command.Parameters.AddWithValue("@ev", performers[dgPerformers.SelectedIndex].Ev);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lblDate.Content = reader.GetValue(0).ToString().Split(" ")[0];
            }
            CloseDatabaseConnection();
        }
    }
}
