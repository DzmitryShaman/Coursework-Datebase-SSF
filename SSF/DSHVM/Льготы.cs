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
    public partial class Льготы : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds;
        public Льготы()
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
                SqlCommand command = new SqlCommand("Select * from Льготы", connection);
                adapter = new SqlDataAdapter(command);

                ds = new DataSet();
                adapter.Fill(ds, "[Льготы]");
                dataGridView1.DataSource = ds.Tables[0];

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(1));
                }

                connection.Close();
            }

            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 320;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"INSERT INTO [Льготы] ([Наименование льготы]) VALUES (N'{textBox1.Text}')", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            update();
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
                SqlCommand command = new SqlCommand("DELETE FROM [Льготы] WHERE [Наименование льготы] = '" + comboBox1.Text + "'", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            update();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter($"select * from Льготы where [Наименование льготы] like '{textBox2.Text}%'", connection);
                ds = new DataSet();
                adapter.Fill(ds, "[Льготы]");
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
    }
}

