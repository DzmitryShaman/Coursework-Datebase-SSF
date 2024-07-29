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
using System.Windows.Shapes;

namespace DSHVM
{
    public partial class Инспектора : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds;
        public Инспектора()
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
                SqlCommand command = new SqlCommand("Select * from Инспектора", connection);
                adapter = new SqlDataAdapter(command);

                ds = new DataSet();
                adapter.Fill(ds, "[Инспектора]");
                dataGridView1.DataSource = ds.Tables[0];

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(1));
                }

                connection.Close();
            }

            dataGridView1.Columns[0].Width = 170;
            dataGridView1.Columns[1].Width = 170;
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[3].Width = 170;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"INSERT INTO [Инспектора] ([ФИО Инспектора],[Телефон инспектора],[Номер участка]) VALUES (N'{textBox1.Text}',N'{textBox4.Text}',N'{textBox3.Text}')", connection);
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
                SqlCommand command = new SqlCommand("DELETE FROM [Инспектора] WHERE [ФИО Инспектора] = '" + comboBox1.Text + "'", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            update();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter($"select * from Инспектора where [ФИО Инспектора] like '{textBox2.Text}%'", connection);
                ds = new DataSet();
                adapter.Fill(ds, "[Инспектора]");
                dataGridView1.DataSource = ds.Tables[0];
                
            }
        }
    }
}
