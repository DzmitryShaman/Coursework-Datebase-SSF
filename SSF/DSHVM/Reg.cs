using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSHVM
{
    public partial class Reg : Form
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point lastPoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            loginField.BackColor = Color.White;
            passField.BackColor = SystemColors.Control;
            textBox2.BackColor = SystemColors.Control;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
            passField.BackColor = Color.White;
            textBox1.BackColor = SystemColors.Control;
            loginField.BackColor = SystemColors.Control;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login frm1 = new Login();
            frm1.Show();
            Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM log WHERE Login=@login", connection);
            command.Parameters.AddWithValue("@login", textBox1.Text);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Этот пользователь уже зарегистрирован");
            }
            else
            {
                command = new SqlCommand("INSERT INTO log (login, pass) VALUES (@login,@pass)", connection);
                command.Parameters.AddWithValue("@login", textBox1.Text);
                command.Parameters.AddWithValue("@pass", textBox2.Text);
                
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Новый пользователь успешно создан");
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации");
                }
            }          
            connection.Close();
        } 
    }
}
