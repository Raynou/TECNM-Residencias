namespace TECNM.Residencias.Forms.CareerForms;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TECNM.Residencias.Data.Entities;
using TECNM.Residencias.Extensions;
using TECNM.Residencias.Forms.SpecialtyForms;

public sealed partial class CareerListViewForm : Form
{
    public CareerListViewForm()
    {
        InitializeComponent();
        Text = $"Listado de carreras | {App.Name}";
        dgv_ListView.DoubleBuffered(true);
    }

    private void CareerListViewForm_Load(object sender, EventArgs e)
    {
        RefreshList();
    }

    private void ListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        var grid = (DataGridView) sender;
        if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            var career = (Career) grid.Rows[e.RowIndex].Tag!;

            if (e.ColumnIndex == 4)
            {
                ShowCareerEditDialog(career);
            }

            if (e.ColumnIndex == 5)
            {
                using var dialog = new SpecialtyListViewForm(career);
                dialog.ShowDialog();
            }
        }
    }

    private void AddNewCareer_Click(object sender, EventArgs e)
    {
        ShowCareerEditDialog();
    }

    private void ShowCareerEditDialog(Career? career = null)
    {
        using var dialog = new CareerEditForm(career);
        dialog.ShowDialog();
        RefreshList();
    }

    private void RefreshList()
    {
        using var context = new AppDbContext();
        IEnumerable<Career> careers = context.Careers.EnumerateAll();

        dgv_ListView.Rows.Clear();

        foreach (Career career in careers)
        {
            int index = dgv_ListView.Rows.Add();
            DataGridViewRow row = dgv_ListView.Rows[index];

            row.Tag = career;
            row.Cells[0].Value = career.Name;
            row.Cells[1].Value = career.Enabled;
            row.Cells[2].Value = career.UpdatedOn.ToString("g");
            row.Cells[3].Value = career.CreatedOn.ToString("g");
        }

        dgv_ListView.ClearSelection();
        UpdateStatusLabel();
    }

    private void UpdateStatusLabel()
    {
        lbl_StatusLabel.Text = $"Número de registros: {dgv_ListView.RowCount}";
    }
}
