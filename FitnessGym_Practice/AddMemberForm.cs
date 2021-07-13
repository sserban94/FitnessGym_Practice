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

namespace FitnessGym_Practice
{
    public partial class AddMemberForm : Form
    {
        public AddMemberForm()
        {
            InitializeComponent();
            DisplayTypeOfMembership();
        }

        public AddMemberForm(Membership membership)
        {
            InitializeComponent();
            DisplayTypeOfMembership();
        }

        private void DisplayTypeOfMembership()
        {
            cbMembershipType.Items.Add(TypeOfMembershipEnum.StudentMembership);
            cbMembershipType.Items.Add(TypeOfMembershipEnum.DayTimeMembership);
            cbMembershipType.Items.Add(TypeOfMembershipEnum.FullTimeMembership);
        }

        private void bntInsertMember_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show(
                    "Validations failed",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            string firstName = tbFirstName.Text.Trim();
            string lastName = tbLastName.Text.Trim();
            TypeOfMembershipEnum membershipType = (TypeOfMembershipEnum)cbMembershipType.SelectedItem;
            long price = long.Parse(tbPrice.Text);
            Membership member = new Membership(firstName, lastName, membershipType, price);
            MainForm.fitnessGym.Memberships.Add(member);

            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbPrice.Text = "";
            cbMembershipType.SelectedIndex = -1;
            
        }

        private void tbFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (tbFirstName.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbFirstName, "Field must not be empty");
                e.Cancel = true;
            }
            
        }

        private void tbFirstName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(tbFirstName, null);
        }

        private void tbLastName_Validating(object sender, CancelEventArgs e)
        {
            if (tbLastName.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbLastName, "Field must not be empty");
                e.Cancel = true;
            }
            
        }

        private void tbLastName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(tbLastName, null);
        }

        private void cbMembershipType_Validating(object sender, CancelEventArgs e)
        {
            if (cbMembershipType.SelectedIndex == -1)
            {
                errorProvider.SetError(cbMembershipType, "Must choose a type");
                e.Cancel = true;
            }
            
        }

        private void cbMembershipType_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(cbMembershipType, null);
        }

        private void tbPrice_Validating(object sender, CancelEventArgs e)
        {
            if (tbPrice.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbPrice, "Field must not be empty");
                e.Cancel = true;
            }
            else if (int.Parse(tbPrice.Text) == 0)
            {
                errorProvider.SetError(tbPrice, "No free memberships");
                e.Cancel = true;
            }
            
        }

        private void tbPrice_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(tbPrice, null);
        }

        private void tbFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
