using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSHVM
{
    public partial class Edit : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds;
        SqlConnection sqlConnection;
        public string[] tempDate { get; set; }
        public Edit()
        {
            InitializeComponent();

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string q = "SELECT * FROM Инспектора";
            SqlCommand command = new SqlCommand(q, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(1));
            }

            sqlConnection.Close();
            sqlConnection.Open();
            command = new SqlCommand("SELECT * From Льготы", sqlConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader.GetString(1));
            }
            sqlConnection.Close();
            sqlConnection.Open();
            command = new SqlCommand("SELECT * From Банки", sqlConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetString(1));
            }
            sqlConnection.Close();
            update();
        }

        private void update()
        {
            comboBox4.Items.Clear();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("SELECT Фамилия, Имя, Отчество, УНП FROM [Соц защита] JOIN Клиенты ON [Соц защита].[Код Клиента]=Клиенты.[Код Клиента]", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox4.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " УНП: " + reader.GetInt32(3));
            }
            sqlConnection.Close();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null) return;
            tempDate = comboBox4.SelectedItem.ToString().Split(' ');
            SqlCommand command = new SqlCommand($"SELECT ID, Фамилия, Имя, Отчество, УНП, УНПФ, [Дата регистрации в ИМНС], [Дата постановки на учёт в ФСЗН], Город, Телефон, [Расчётный счёт], [Наименование льготы], [Наименование банка], [ФИО Инспектора] FROM [Соц защита] JOIN Клиенты ON [Соц защита].[Код Клиента]=Клиенты.[Код Клиента] JOIN Льготы ON [Соц защита].[Код Льготы]=Льготы.[Код Льготы] JOIN Банки ON [Соц защита].[Код Банка]=Банки.[Код Банка] JOIN Инспектора ON [Соц защита].[Код Инспектора]=Инспектора.[Код Инспектора] WHERE Фамилия = '{tempDate[0]}' AND Имя = '{tempDate[1]}' AND Отчество = '{tempDate[2]}' AND УНП = '{tempDate[4]}'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader.GetString(2);
                textBox2.Text = reader.GetString(1);
                textBox3.Text = reader.GetString(3);
                textBox4.Text = reader.GetInt32(4).ToString();
                textBox5.Text = reader.GetInt32(5).ToString();
                textBox6.Text = reader.GetDateTime(6).ToString("dd.MM.yyyy");
                textBox7.Text = reader.GetDateTime(7).ToString("dd.MM.yyyy");
                textBox8.Text = reader.GetString(8);
                textBox9.Text = reader.GetString(9);
                textBox10.Text = reader.GetInt32(10).ToString();
                comboBox3.Text = reader.GetString(11);
                comboBox2.Text = reader.GetString(12);
                comboBox1.Text = reader.GetString(13);
            }
            sqlConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            SqlCommand command = new SqlCommand($"UPDATE [Клиенты] SET Фамилия = '{textBox2.Text}', Имя = '{textBox1.Text}', Отчество = '{textBox3.Text}', Город = '{textBox8.Text}', Телефон = '{textBox9.Text}' WHERE Фамилия = '{tempDate[0]}' AND Имя = '{tempDate[1]}' AND Отчество = '{tempDate[2]}'", sqlConnection);
            command.ExecuteNonQuery();
            command = new SqlCommand($"SELECT [Код клиента] From [Клиенты] WHERE Фамилия = '{textBox2.Text}' AND Имя = '{textBox1.Text}' AND Отчество = '{textBox3.Text}'", sqlConnection);
            int clientID = Convert.ToInt32(command.ExecuteScalar());
            command = new SqlCommand($"SELECT [Код льготы] From [Льготы] WHERE [Наименование льготы] = '{comboBox3.SelectedItem.ToString()}'", sqlConnection);
            int lgotID = (int)command.ExecuteScalar();
            command = new SqlCommand($"SELECT [Код банка] From [Банки] WHERE [Наименование банка] = '{comboBox2.SelectedItem.ToString()}'", sqlConnection);
            int bankID = (int)command.ExecuteScalar();
            command = new SqlCommand($"SELECT [Код инспектора] From [Инспектора] WHERE [ФИО Инспектора] = '{comboBox1.SelectedItem.ToString()}'", sqlConnection);
            int inspID = (int)command.ExecuteScalar();
            command = new SqlCommand($"UPDATE [Соц защита] SET УНП = {textBox4.Text}, УНПФ = {textBox5.Text}, [Дата регистрации в ИМНС] = '{DateTime.Parse(textBox6.Text)}', [Дата постановки на учёт в ФСЗН] = '{DateTime.Parse(textBox7.Text)}', [Расчётный счёт] = {textBox10.Text}, [Код льготы] = {lgotID},[Код банка] = {bankID}, [Код инспектора] = {inspID} WHERE [Код клиента] = {clientID}", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
            comboBox4.SelectedIndex = -1;
            update();
        }
    }
}
