using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DSHVM
{
    public partial class Регистрация : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlConnection sqlConnection;
        public Регистрация()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //      SqlCommand command = new SqlCommand($"INSERT INTO [Соц защита] (Фамилия, Имя, Отчество, УНП, УНПФ, [Дата регистрации ИМНС], [Дата постановки на учёт в ФСЗН], Город, Телефон, [Расчётный счёт], Льгота, [Наименование банка],) VALUES (N'{textBox2.Text}',N'{textBox1.Text}',N'{textBox3.Text}',N'{textBox4.Text}',N'{textBox5.Text}',N'{textBox6.Text}',N'{textBox7.Text}',N'{textBox8.Text}',N'{textBox9.Text}',N'{textBox10.Text}',N'{textBox11.Text}',N'{textBox14.Text}')", sqlConnection);
            //      MessageBox.Show(command.ExecuteNonQuery().ToString());
            sqlConnection.Close();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand($"INSERT INTO [Клиенты] (Фамилия, Имя, Отчество, Город, Телефон) VALUES (N'{textBox2.Text}',N'{textBox1.Text}',N'{textBox3.Text}',N'{textBox8.Text}',N'{textBox9.Text}'); SELECT SCOPE_IDENTITY()", sqlConnection);
            int clientID = Convert.ToInt32(command.ExecuteScalar());
            command = new SqlCommand($"SELECT [Код льготы] From [Льготы] WHERE [Наименование льготы] = '{comboBox3.SelectedItem.ToString()}'", sqlConnection);
            int lgotID = (int)command.ExecuteScalar();
            command = new SqlCommand($"SELECT [Код банка] From [Банки] WHERE [Наименование банка] = '{comboBox2.SelectedItem.ToString()}'", sqlConnection);
            int bankID = (int)command.ExecuteScalar();
            command = new SqlCommand($"SELECT [Код инспектора] From [Инспектора] WHERE [ФИО Инспектора] = '{comboBox1.SelectedItem.ToString()}'", sqlConnection);
            int inspID = (int)command.ExecuteScalar();
            command = new SqlCommand($"INSERT INTO [Соц защита] ([Код клиента],УНП, УНПФ, [Дата регистрации в ИМНС], [Дата постановки на учёт в ФСЗН], [Расчётный счёт], [Код льготы],[Код банка], [Код инспектора]) VALUES ({clientID},{textBox4.Text},{textBox5.Text},'{DateTime.Parse(textBox6.Text)}','{DateTime.Parse(textBox7.Text)}',{textBox10.Text},{lgotID},{bankID},{inspID})", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
            MessageBox.Show("Клиент успешно добавлен");
        }
    }
}
