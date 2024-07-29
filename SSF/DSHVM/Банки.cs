using FontAwesome.Sharp;
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
    public partial class Банки : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds;
        public Банки()
        {
            InitializeComponent();
            
            update();
        }

        private void update()
        {
            comboBox1.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * from Банки", connection);
                adapter = new SqlDataAdapter(command);

                ds = new DataSet();
                adapter.Fill(ds, "[Банки]");
                dataGridView1.DataSource = ds.Tables[0];

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(1));
                }

                connection.Close();
            }

            dataGridView1.Columns[0].Width = 155;
            dataGridView1.Columns[1].Width = 175;
            dataGridView1.Columns[2].Width = 180;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[4].Width = 180;
            dataGridView1.Columns[5].Width = 175;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM [Банки] WHERE [Наименование банка] = '" + comboBox1.Text + "'", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"INSERT INTO [Банки] ([Наименование банка],[УНП банка],[БИК банка],[Адрес банка],[Телефон банка]) VALUES (N'{textBox5.Text}',N'{textBox4.Text}',N'{textBox3.Text}',N'{textBox2.Text}',N'{textBox1.Text}')", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            update();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter($"select * from Банки where [Наименование банка] like '{textBox6.Text}%'", connection);
                ds = new DataSet();
                adapter.Fill(ds, "[Банки]");
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
    }
}
