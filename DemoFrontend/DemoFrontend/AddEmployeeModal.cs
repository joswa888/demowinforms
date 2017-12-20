using System;
using System.Windows.Forms;

namespace DemoFrontend
{
    public partial class AddEmployeeModal : Form
    {
        public AddEmployeeModal()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtBoxFirstName.Text.Trim().Length < 1)
                MessageBox.Show("First name field is required.");

            else if (txtBoxLastName.Text.Trim().Length < 1)
                MessageBox.Show("Last name field is required.");

            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtBoxFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar));
        }

        private void txtBoxLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar));
        }

        public string FirstName
        {
            get { return txtBoxFirstName.Text.Trim(); }
        }

        public string LastName
        {
            get { return txtBoxLastName.Text.Trim(); }
        }


    }
}
