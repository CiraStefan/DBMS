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

namespace Lab1
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter daDrivers, daGrandPrixes;
        DataSet ds;
        SqlCommandBuilder cb;
        BindingSource bsDrivers, bsGrandPrixes;



        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source = STEFANCIRA\SQLEXPRESS; Initial Catalog = Formula1; Integrated Security = True");
            ds = new DataSet();
            daDrivers = new SqlDataAdapter("select * from Driver", connection);
            daGrandPrixes = new SqlDataAdapter("select * from GrandPrix", connection);
            daDrivers.Fill(ds, "Driver");
            daGrandPrixes.Fill(ds, "GrandPrix");

            cb = new SqlCommandBuilder(daDrivers);
            
            DataRelation dr = new DataRelation("FK__GrandPrix__winne__47DBAE45", ds.Tables["Driver"].Columns["did"], ds.Tables["GrandPrix"].Columns["winner"]);
            ds.Relations.Add(dr);

            bsDrivers = new BindingSource();
            bsGrandPrixes = new BindingSource();

            bsDrivers.DataSource = ds;
            bsDrivers.DataMember = "Driver";

            bsGrandPrixes.DataSource = bsDrivers;
            bsGrandPrixes.DataMember = "FK__GrandPrix__winne__47DBAE45";
            connection.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            DriverGrid.DataSource = bsDrivers;
            GrandPrixGrid.DataSource = bsGrandPrixes;
            connection.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void GrandPrixGrid_Click(object sender, EventArgs e)
        {
            textBox2.Text = GrandPrixGrid.SelectedCells[0].Value.ToString();
            textBox3.Text = GrandPrixGrid.SelectedCells[1].Value.ToString();
            textBox4.Text = GrandPrixGrid.SelectedCells[2].Value.ToString();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connection.Open();
            if (textBox2.Text == null || textBox3.Text == null || textBox4.Text == null)
            {
                MessageBox.Show("Please input some inputs", "Gata vrajeala", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var location = textBox2.Text;
                var season = textBox3.Text;
                string deleteQuery = "delete from GrandPrix where locatione = '" + location + "' and season = '" + season + "'";
                SqlCommand cmd = new SqlCommand(deleteQuery, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("N-a mers smecheria", "Nu se poate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            bsDrivers.DataSource = ds;
            bsDrivers.DataMember = "Driver";

            bsGrandPrixes.DataSource = bsDrivers;
            bsGrandPrixes.DataMember = "FK__GrandPrix__winne__47DBAE45";
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //connection = new SqlConnection(@"Data Source = STEFANCIRA\SQLEXPRESS; Initial Catalog = Formula1; Integrated Security = True");
            connection.Open();
            var location = textBox2.Text;
            var season = Convert.ToInt32(textBox3.Text);
            var winner = textBox4.Text;
            string Queey = "insert into GrandPrix values ('"+location+"',"+season+", '"+winner+"')";
            //string updateQuery2 = "update GrandPrix set location = '" + location + "' where locatione like '" + location + "' and season like '" + season + "'";
            //string updateQuery3 = "update GrandPrix set winner = '" + winner + "' where locatione like '" + location + "' and season like '" + season + "'";
            SqlCommand cmd = new SqlCommand(Queey, connection);
            try
            {
                cmd.ExecuteNonQuery();
                daGrandPrixes.Update(ds, "GrandPrix");
                GrandPrixGrid.DataSource = bsGrandPrixes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "N-a mers smecheria", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                if (location == null || season == null || winner == null) 
                    MessageBox.Show(ex.Message, "N-a mers smecheria", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    executeUpdateCommand();
            }
        }

        private void executeUpdateCommand()
        {
            connection.Open();
            var location = textBox2.Text;
            var season = textBox3.Text;
            var winner = textBox4.Text;
            string updateQuery1 = "update GrandPrix set winner = '" + winner + "' where locatione like '" + location + "' and season like '" + season + "'";
            SqlCommand cmd1 = new SqlCommand(updateQuery1, connection);
            cmd1.ExecuteNonQuery();
            GrandPrixGrid.Update();

            bsGrandPrixes.DataSource = bsDrivers;
            bsGrandPrixes.DataMember = "FK__GrandPrix__winne__47DBAE45";
            //bsGrandPrixes.
            daGrandPrixes.Update(ds, "GrandPrix");
            GrandPrixGrid.DataSource = bsGrandPrixes;
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
