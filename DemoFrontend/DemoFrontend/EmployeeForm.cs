using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DemoBackend.Contracts.Responses;
using System.Linq;
using System.Windows.Forms;
using DemoHelpers;
using static DemoHelpers.CollectionHelpers;
using DemoBackend.Contracts.Requests;

namespace DemoFrontend
{
    public partial class EmployeeForm : XtraForm
    {
        private ObservableRangeCollection<GetEmployeeResponse> EmployeeDataSource  { get; set; }
        public EmployeeForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            gridControl.DataSource = GetDataSource();
            gridControl.RefreshDataSource();
        }

        void windowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch(e.Button.Properties.Caption)
            {
                case "Print":
                    gridControl.ShowRibbonPrintPreview();
                    break;

                case "New":
                    

                    using (var addEmployeeModal = new AddEmployeeModal())
                    {
                        if (addEmployeeModal.ShowDialog(this) == DialogResult.OK)
                        {
                            var createEmployeeRequest = new UpsertEmployeeRequest
                            {
                                FirstName = addEmployeeModal.FirstName,
                                LastName = addEmployeeModal.LastName
                            };

                            var createEmployee = Task.Run(() => HttpRequests.CreateEmployee(createEmployeeRequest));

                            createEmployee.Wait();
                        }
                    }

                    RefreshGrid();

                    break;

                case "Edit":
                    var latestEmployeeData = (GetEmployeeResponse) GetClickedRow();

                    if (latestEmployeeData == null)
                        break;

                    var upsertEmployeeRequest = new UpsertEmployeeRequest
                    {
                        FirstName = latestEmployeeData.FirstName,
                        LastName = latestEmployeeData.LastName
                    };

                    var updateEmployee = Task.Run(() => HttpRequests.UpdateEmployee(latestEmployeeData.Id, upsertEmployeeRequest));
                    updateEmployee.Wait();
                    RefreshGrid();

                    break;

                case "Delete":
                    var deleteEmployeeRequest = (GetEmployeeResponse)GetClickedRow();

                    if (deleteEmployeeRequest == null)
                        break;

                    var deleteEmployee = Task.Run(() => HttpRequests.DeleteEmployee(deleteEmployeeRequest.Id));
                    deleteEmployee.Wait();
                    RefreshGrid();

                    break;

                case "Refresh":
                    RefreshGrid();
                    gridControl.RefreshDataSource();
                    break;

            }
        }
        public ObservableRangeCollection<GetEmployeeResponse> GetDataSource()
        {
            var employees = Task.Run( () => HttpRequests.GetEmployees());

            try
            {
                employees.Wait();
            }
            catch
            {
                MessageBox.Show(string.Format("Connection to Server Status: {0}", employees.Status.ToString()));
                return null;
            }
            

            var result = new ObservableRangeCollection<GetEmployeeResponse>();
            result.AddRange(employees.Result);
            
            return result;
        }

        private object GetClickedRow()
        {
            var selectedRowIndex = gridView.GetSelectedRows().FirstOrDefault();
            var rowValue = gridControl.FocusedView.GetRow(selectedRowIndex);
            return rowValue;
        }

        private void gridControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var rowValue = (GetEmployeeResponse)GetClickedRow();
            var parseToString = StringHelpers.ObjectToString(rowValue);
            MessageBox.Show(parseToString);
        }

        private void gridControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char) Keys.Enter ))
            {

            }
        }
    }
}
