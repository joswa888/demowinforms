using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DemoBackend.Contracts.Responses;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.EditForm.Helpers.Controls;
using DemoBackend.Contracts.Requests;

namespace DemoFrontend.Forms
{
    public partial class EmployeeForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public EmployeeForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            RefreshGrid();
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void RefreshGrid()
        {
            EmployeeDataSource = GetDataSource() ?? new BindingList<GetEmployeeResponse>();
            gridControl.DataSource = EmployeeDataSource;
            gridControl.RefreshDataSource();
            bsiRecordsCount.Caption = "RECORDS : " + EmployeeDataSource?.Count;
        }
        
        private BindingList<GetEmployeeResponse> EmployeeDataSource { get; set; }
        public BindingList<GetEmployeeResponse> GetDataSource()
        {
            var getEmployeeList = Task.Run(() => HttpRequests.GetEmployees());

            try
            {
                getEmployeeList.Wait();
            }
            catch
            {
                MessageBox.Show(string.Format("Connection to Server Status: {0}", getEmployeeList.Status.ToString()));
                return null;
            }

            var result = new BindingList<GetEmployeeResponse>();

            foreach (var employee in getEmployeeList.Result)
            {
                result.Add(employee);   
            }

            return result;
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshGrid();
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView.AddNewRow();
            gridView.ShowEditForm();
            RefreshGrid();
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.SelectedRowsCount > 0)
            {
                gridView.ShowEditForm();
            }

            else
                NothingToModify();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.SelectedRowsCount > 0)
            {
                if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //delete call to API
                    var deleteEmployeeRequest = (GetEmployeeResponse)GetFocusedRow();

                    if (deleteEmployeeRequest == null)
                        return;

                    var deleteEmployee = Task.Run(() => HttpRequests.DeleteEmployee(deleteEmployeeRequest.Id));

                    deleteEmployee.Wait();


                    //delete in gridview 
                    gridView.DeleteSelectedRows();
                }
            }

            else
                NothingToModify();
            
        }

        private void NothingToModify()
        {
            MessageBox.Show("Table is empty. Nothing to modify.");
        }

        private object GetFocusedRow()
        {
            var selectedRowIndex = gridView.GetSelectedRows().FirstOrDefault();
            var rowValue = gridControl.FocusedView.GetRow(selectedRowIndex);
            return rowValue;
        }

        private void gridView_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            GridColumn column = (e as EditFormValidateEditorEventArgs)?.Column ?? view.FocusedColumn;
            if (!(column.Name.Equals("firstname", StringComparison.CurrentCultureIgnoreCase) 
                || column.Name.Equals("lastname", StringComparison.CurrentCultureIgnoreCase)) )
                return;

            var inputValue = Convert.ToString(e.Value);
            var inputValueArray = inputValue.ToCharArray();
            
            if (! (inputValue.All(c => Char.IsLetter(c) || c == ' ')))
                e.Valid = false;
        }

        private void gridView_ShowingPopupEditForm(object sender, DevExpress.XtraGrid.Views.Grid.ShowingPopupEditFormEventArgs e)
        {
            foreach (Control control in e.EditForm.Controls)
            {
                if (!(control is EditFormContainer))
                {
                    continue;
                }
                foreach (Control nestedControl in control.Controls)
                {
                    if (!(nestedControl is PanelControl))
                    {
                        continue;
                    }
                    foreach (Control button in nestedControl.Controls)
                    {
                        if (!(button is SimpleButton))
                        {
                            continue;
                        }
                        var simpleButton = button as SimpleButton;
                        simpleButton.Click -= editFormUpdateButton_Click;
                        simpleButton.Click += editFormUpdateButton_Click;
                    }
                }
            }
        }
        private void editFormUpdateButton_Click(object sender, EventArgs e)
        {
            var btnType = sender.GetType().Name;

            if (btnType.Equals("EditFormCancelButton", StringComparison.CurrentCultureIgnoreCase))
                return;

            var upsertEmployeeRequest = (GetEmployeeResponse) GetFocusedRow();

            UpsertEmployee(upsertEmployeeRequest);
        }

        private void UpsertEmployee(GetEmployeeResponse request)
        {
            var upsertEmployeeRequest = new UpsertEmployeeRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            if (request.Id > 0)
            {
                var updateEmployee = Task.Run(() => HttpRequests.UpdateEmployee(request.Id, upsertEmployeeRequest));

                updateEmployee.Wait();
            }

            else
            {
                var createEmployee = Task.Run(() => HttpRequests.CreateEmployee(upsertEmployeeRequest));

                createEmployee.Wait();
            }
        }

        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["Id"], 0 );
            view.SetRowCellValue(e.RowHandle, view.Columns["DateCreated"], DateTime.UtcNow);
        }
    }
}