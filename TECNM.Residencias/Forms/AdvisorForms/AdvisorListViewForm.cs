using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TECNM.Residencias.Data.Entities;

namespace TECNM.Residencias.Forms.AdvisorForms
{
    public sealed partial class AdvisorListViewForm : Form
    {
        private Company _company = new Company();

        public AdvisorListViewForm()
        {
            InitializeComponent();
            Text = $"Listado de asesores | DEBUG COMPANY | {App.Name}";
        }

        public AdvisorListViewForm(Company company) : this()
        {
            Text = $"Listado de asesores | {company.Name} | {App.Name}";
            _company = company;
        }

        private void AdvisorListViewForm_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void ListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView) sender;
            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var advisor = (Advisor) grid.Rows[e.RowIndex].Tag!;
                ShowAdvisorEditDialog(advisor);
            }
        }

        private void AddNewAdvisor_Click(object sender, EventArgs e)
        {
            ShowAdvisorEditDialog();
        }

        private void ShowAdvisorEditDialog(Advisor? advisor = null)
        {
            using var dialog = new AdvisorEditForm(_company, advisor);
            dialog.ShowDialog();
            RefreshList();
        }

        private void RefreshList()
        {
            using var context = new AppDbContext();
            IEnumerable<Advisor> advisors = context.Advisors.EnumerateAdvisorsByCompany(_company.Id);
            PopulateTable(context, advisors);
        }

        private void PopulateTable(AppDbContext context, IEnumerable<Advisor> advisors)
        {
            dgv_ListView.Rows.Clear();
            int count = 0;

            foreach (Advisor advisor in advisors)
            {
                int index = dgv_ListView.Rows.Add();
                DataGridViewRow row = dgv_ListView.Rows[index];

                row.Tag = advisor;
                row.Cells[0].Value = advisor.Name;
                row.Cells[1].Value = TranslateAdvisorTypeEnum(advisor.Type);
                row.Cells[2].Value = context.Companies.GetCompanyNameById(advisor.CompanyId);
                row.Cells[3].Value = advisor.Section;
                row.Cells[4].Value = advisor.Role;
                row.Cells[5].Value = advisor.Email;
                row.Cells[6].Value = advisor.Phone;
                row.Cells[7].Value = advisor.Enabled;
                row.Cells[8].Value = advisor.UpdatedOn;
                row.Cells[9].Value = advisor.CreatedOn;
                count++;
            }

            dgv_ListView.ClearSelection();
        }

        private string TranslateAdvisorTypeEnum(AdvisorType type)
        {
            return type switch
            {
                AdvisorType.Internal => "Interno",
                AdvisorType.External => "Externo",
                _ => throw new UnreachableException(),
            };
        }
    }
}
