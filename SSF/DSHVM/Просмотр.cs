using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using MySqlX.XDevAPI.Common;
using System.Windows.Controls;

namespace DSHVM
{
    public partial class Просмотр : Form
    {
        int pageSize = 5; // размер страницы
        int pageNumber = 0; // текущая страница
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds;
        public Просмотр()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);

                ds = new DataSet();
                adapter.Fill(ds, "[Соц защита]");
                dataGridView1.DataSource = ds.Tables[0];
                //dataGridView1.Columns["Id"].ReadOnly = true;
            }
            dataGridView1.Columns[0].Width = 150;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 170;
            dataGridView1.Columns[4].Width = 170;
            dataGridView1.Columns[5].Width = 170;
            dataGridView1.Columns[6].Width = 130;
            dataGridView1.Columns[7].Width = 130;
            dataGridView1.Columns[8].Width = 150;
            dataGridView1.Columns[9].Width = 150;
            dataGridView1.Columns[10].Width = 140;
            dataGridView1.Columns[11].Width = 160;

            dataGridView1.Columns[0].Visible = false;

            //dataGridView1.RowHeadersVisible = false;
        }
        private string GetSql()
        {
            //return "SELECT * FROM People ORDER BY Id OFFSET ((" + pageNumber + ") * " + pageSize + ") " +
            //    "ROWS FETCH NEXT " + pageSize + "ROWS ONLY";
            return "SELECT ID, Фамилия, Имя, Отчество, УНП, УНПФ, [Дата регистрации в ИМНС], [Дата постановки на учёт в ФСЗН], Город, [Расчётный счёт],[Наименование банка], [ФИО Инспектора] FROM [Соц защита] JOIN Клиенты ON [Соц защита].[Код Клиента]=Клиенты.[Код Клиента] JOIN Льготы ON [Соц защита].[Код Льготы]=Льготы.[Код Льготы] JOIN Банки ON [Соц защита].[Код Банка]=Банки.[Код Банка] JOIN Инспектора ON [Соц защита].[Код Инспектора]=Инспектора.[Код Инспектора]";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (ds.Tables["People"].Rows.Count < pageSize) return;

            pageNumber++;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);

                ds.Tables["People"].Rows.Clear();

                adapter.Fill(ds, "People");
            }
        }
        private string result = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (pageNumber == 0) return;
            pageNumber--;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);

                ds.Tables["People"].Rows.Clear();

                adapter.Fill(ds, "People");
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e) // Метод печати для printDocument
        {
            Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 10);
            dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
            e.Graphics.DrawImage(bmp, 0, 0);
        }
        private void iconButton2_Click(object sender, EventArgs e)
        {
            /*PrintDocument printDocument = new PrintDocument();
             printDocument.PrintPage += PrintPageHandler;
             PrintDialog printDialog = new PrintDialog();
             printDialog.Document = printDocument;
             if (printDialog.ShowDialog() == DialogResult.OK)
                 printDialog.Document.Print();
             Interop.Excel();*/
        }
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(result, new Font("Arial", 14), Brushes.Black, 0, 0);
        }

        private void icondelete()
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            //    icondelete();
            //int rowIndex = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
            if (MessageBox.Show("Вы действительно хотите удалить?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["ID"].Value);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM [Соц защита] WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                dataGridView1.Rows.RemoveAt(rowIndex);
                dataGridView1.Refresh();
                connection.Close();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
            string newValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            var q = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string headerText = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand($"UPDATE [Клиенты] SET Where [Код клиента] = ", connection);
                //SqlCommand command = new SqlCommand("UPDATE YourTableName SET YourColumnName = @newValue WHERE id = @rowId", connection);
                SqlCommand command = new SqlCommand($"UPDATE [Соц защита] SET {headerText} = {newValue} FROM [Соц зашита] WHERE [Соц защита].ID = {rowId}", connection);
                command.Parameters.AddWithValue("@newValue", newValue);
                command.Parameters.AddWithValue("@rowId", rowId);
                int rowsAffected = command.ExecuteNonQuery();
            }
            dataGridView1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter("select * from [Соц защита] JOIN Клиенты ON [Соц защита].[Код Клиента]=Клиенты.[Код Клиента] where Фамилия like '" + textBox1.Text + "%' or Имя like '" + textBox1.Text + "%' or Отчество like '" + textBox1.Text + "%' or УНП like '" + textBox1.Text + "%' ", connection);
                ds = new DataSet();
                adapter.Fill(ds, "[Соц защита]");
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }
    }
}

