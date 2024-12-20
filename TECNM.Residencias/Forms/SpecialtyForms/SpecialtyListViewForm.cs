namespace TECNM.Residencias.Forms.SpecialtyForms;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TECNM.Residencias.Data.Entities;
using TECNM.Residencias.Extensions;

public sealed partial class SpecialtyListViewForm : Form
{
    private Career _career = new Career();

    public SpecialtyListViewForm()
    {
        InitializeComponent();
        dgv_ListView.DoubleBuffered(true);
    }

    public SpecialtyListViewForm(Career career) : this()
    {
        Text = $"Especialidades | {career.Name} | {App.Name}";
        _career = career;
    }

    private void SpecialtyListViewForm_Load(object sender, EventArgs e)
    {
        RefreshList();
    }

    private void ListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        var grid = (DataGridView) sender;
        if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            var specialty = (Specialty) grid.Rows[e.RowIndex].Tag!;
            if (e.ColumnIndex == 4)
            {
                ShowSpecialtyEditDialog(_career, specialty);
            }
        }
    }

    private void AddNewSpecialty_Click(object sender, EventArgs e)
    {
        ShowSpecialtyEditDialog(_career);
    }

    private void ShowSpecialtyEditDialog(Career career, Specialty? specialty = null)
    {
        using var dialog = new SpecialtyEditForm(career, specialty);
        dialog.ShowDialog();
        RefreshList();
    }

    private void RefreshList()
    {
        using var context = new AppDbContext();
        IEnumerable<Specialty> specialties = context.Specialties.EnumerateAll(_career);

        dgv_ListView.Rows.Clear();

        foreach (Specialty specialty in specialties)
        {
            int index = dgv_ListView.Rows.Add();
            DataGridViewRow row = dgv_ListView.Rows[index];

            row.Tag = specialty;
            row.Cells[0].Value = specialty.Name;
            row.Cells[1].Value = specialty.Enabled;
            row.Cells[2].Value = specialty.UpdatedOn.ToString("g");
            row.Cells[3].Value = specialty.CreatedOn.ToString("g");
        }

        dgv_ListView.ClearSelection();
        UpdateStatusLabel();
    }

    private void UpdateStatusLabel()
    {
        lbl_StatusLabel.Text = $"Número de registros: {dgv_ListView.RowCount}";
    }
}
