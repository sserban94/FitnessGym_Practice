using FitnessGym_Practice.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FitnessGym_Practice
{
    public partial class MainForm : Form
    {
        #region Attributes
        public static FitnessGym fitnessGym;
        public static string connectionString = "data source=database.db";
        #endregion
        public MainForm()
        {
            InitializeComponent();
            fitnessGym = new FitnessGym("EliteGym");
            DisplayMembers();
        }

        #region Methods
        public void DisplayMembers()
        {
            lvGymMembers.Items.Clear();
            //MembershipPriceDescComparer mpdc = new MembershipPriceDescComparer();
            //fitnessGym.Memberships.Sort(mpdc);
            fitnessGym.Memberships.Sort();
            foreach (var member in fitnessGym.Memberships)
            {
                ListViewItem item = new ListViewItem(member.FirstName);
                item.SubItems.Add(member.LastName);
                item.SubItems.Add(member.TypeOfMembership.ToString());
                item.SubItems.Add(member.Price.ToString());

                item.Tag = member;
                lvGymMembers.Items.Add(item);
            }

        }


        #endregion
        private void btnAddMember_Click(object sender, EventArgs e)
        {
            AddMemberForm form = new AddMemberForm();
            form.ShowDialog();
            DisplayMembers();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvGymMembers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose member");
            }

            if (MessageBox.Show(
                "Are you sure?",
                "Delete member",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (ListViewItem lvi in lvGymMembers.SelectedItems)
                {
                    Membership membershiper = (Membership)lvi.Tag;
                    fitnessGym.Memberships.Remove(membershiper);

                }
                //ListViewItem listViewItem = lvGymMembers.SelectedItems[0];
                //Membership membership = (Membership)listViewItem.Tag;
                //fitnessGym.Memberships.Remove(membership);
                DisplayMembers();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // DE CONTINUAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (lvGymMembers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose member");
                return;
            }
            ListViewItem listViewItem = lvGymMembers.SelectedItems[0];
            Membership membership = (Membership)listViewItem.Tag;

            AddMemberForm addMemberForm = new AddMemberForm(membership);
            if (addMemberForm.ShowDialog() == DialogResult.OK)
            {
                DisplayMembers();
            }
        }

        private void btnExportSql_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Memberships (FirstName, LastName, TypeofMembership, Price) VALUES" +
                "(@firstName, @lastName, @typeOfMembership, @price);" +
                "SELECT last_insert_rowid();";
            foreach (Membership member in fitnessGym.Memberships)
            {
                using (SqliteConnection sqliteConnection = new SqliteConnection(connectionString))
                {
                    SqliteCommand sqliteCommand = new SqliteCommand(query, sqliteConnection);
                    sqliteCommand.Parameters.AddWithValue("@firstName", member.FirstName);
                    sqliteCommand.Parameters.AddWithValue("@lastName", member.LastName);
                    sqliteCommand.Parameters.AddWithValue("@typeOfMembership", member.TypeOfMembership.ToString());
                    sqliteCommand.Parameters.AddWithValue("@price", member.Price);

                    sqliteConnection.Open();

                    long id = (long)sqliteCommand.ExecuteScalar();
                    member.Id = id;
                }
            }

        }

        private void btnImportSql_Click(object sender, EventArgs e)
        {
            fitnessGym.Memberships.Clear();
            string query = "SELECT * FROM Memberships";
            using (SqliteConnection sqlConnection = new SqliteConnection(connectionString))
            {
                SqliteCommand sqliteCommand = new SqliteCommand(query, sqlConnection);
                sqlConnection.Open();
                using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Membership member = new Membership();
                        member.Id = (long)reader["Id"];
                        member.FirstName = (string)reader["FirstName"];
                        member.LastName = (string)reader["LastName"];
                        member.TypeOfMembership = (TypeOfMembershipEnum)Enum.Parse(typeof(TypeOfMembershipEnum), reader["TypeOfMembership"].ToString());
                        member.Price = (long)reader["Price"];
                        fitnessGym.Memberships.Add(member);
                    }
                }

            }
            DisplayMembers();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream stream = File.Create("memberships.bin"))
            {
                binaryFormatter.Serialize(stream, fitnessGym.Memberships);
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("memberships.bin"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream stream = File.OpenRead("memberships.bin"))
                {
                    fitnessGym.Memberships.Clear();
                    if (stream.Length == 0)
                    {
                        return;
                    }
                    fitnessGym.Memberships = (List<Membership>)binaryFormatter.Deserialize(stream);
                    DisplayMembers();
                }
            }
        }

       
    }
}
