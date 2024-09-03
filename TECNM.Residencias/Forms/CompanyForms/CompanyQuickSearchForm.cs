using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TECNM.Residencias.Data.Entities;

namespace TECNM.Residencias.Forms.CompanyForms
{
    public sealed partial class CompanyQuickSearchForm : Form
    {
        public Company? SelectedCompany { get; private set; }

        public CompanyQuickSearchForm()
        {
            InitializeComponent();
        }

        private void RunSearch_Click(object sender, EventArgs e)
        {
            SearchCompanies();
        }

        private void SearchQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                SearchCompanies();
                e.Handled = true;
            }
        }

        private void SearchCompanies()
        {
            string query = tb_SearchQuery.Text.Trim();
            if (query.Length == 0)
            {
                return;
            }

            using var context = new AppDbContext();
            IEnumerable<Company> companies = context.Companies.Search(query, 50, 1);

            dgv_ListView.Rows.Clear();

            foreach (Company company in companies)
            {
                int index = dgv_ListView.Rows.Add();
                DataGridViewRow row = dgv_ListView.Rows[index];

                row.Tag = company;
                row.Cells[0].Value = company.Rfc;
                row.Cells[1].Value = company.Name;
            }
        }

        private void ListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView) sender;
            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                SelectedCompany = (Company) grid.Rows[e.RowIndex].Tag!;
                Close();
            }
        }
    }
}
