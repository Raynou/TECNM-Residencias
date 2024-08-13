using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TECNM.Residencias.Data.Entities;
using TECNM.Residencias.Data.Sets;
using TECNM.Residencias.Extensions;

namespace TECNM.Residencias.Forms.StudentForms
{
    public sealed partial class StudentListViewForm : Form
    {
        private readonly int _rowsPerPage = StudentDbSet.DEFAULT_ROWS_PER_PAGE;
        private int _currentPage = StudentDbSet.DEFAULT_INITIAL_PAGE;
        private bool _refreshFromSearch = false;

        public StudentListViewForm()
        {
            InitializeComponent();
            Text = $"Listado de residentes | {App.Name}";
            dgv_ListView.DoubleBuffered(true);
        }

        private void StudentListViewForm_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void ListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView) sender;
            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var student = (Student) grid.Rows[e.RowIndex].Tag!;
                ShowStudentEditDialog(student);
            }
        }

        private void AddNewCompany_Click(object sender, EventArgs e)
        {
            ShowStudentEditDialog();
        }

        private void SearchQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                SearchStudents();
                e.Handled = true;
            }
        }

        private void RunQuerySearch_Click(object sender, EventArgs e)
        {
            SearchStudents();
        }

        private void ResetSearch_Click(object sender, EventArgs e)
        {
            _currentPage = StudentDbSet.DEFAULT_INITIAL_PAGE;
            _refreshFromSearch = false;
            RefreshList();
        }

        private void PagePrev_Click(object sender, EventArgs e)
        {
            _currentPage = Math.Max(0, _currentPage - 1);
            RefreshList();
        }

        private void PageNext_Click(object sender, EventArgs e)
        {
            _currentPage++;
            RefreshList();
        }

        private void ShowStudentEditDialog(Student? student = null)
        {
            using var dialog = new StudentEditForm(student);
            dialog.ShowDialog();
            RefreshList();
        }

        private void RefreshList()
        {
            if (_refreshFromSearch)
            {
                SearchStudents();
                return;
            }

            using var context = new AppDbContext();
            IEnumerable<Student> students = context.Students.EnumerateStudents(_rowsPerPage, _currentPage);
            PopulateTable(context, students);
        }

        private void SearchStudents()
        {
            string query = tb_SearchQuery.Text.Trim();
            if (query.Length == 0)
            {
                return;
            }

            _refreshFromSearch = true;
            using var context = new AppDbContext();
            IEnumerable<Student> students = context.Students.Search(query, _rowsPerPage, _currentPage);
            PopulateTable(context, students);
        }

        private void PopulateTable(AppDbContext context, IEnumerable<Student> students)
        {
            dgv_ListView.Rows.Clear();
            int count = 0;

            foreach (Student student in students)
            {
                int index = dgv_ListView.Rows.Add();
                DataGridViewRow row = dgv_ListView.Rows[index];

                Specialty specialty = context.Specialties.GetSpecialtyById(student.SpecialtyId)!;
                Advisor? internAdvisor;
                Advisor? externAdvisor;
                Advisor? reviewer;

                row.Tag = student;
                row.Cells[0].Value = student.Id;
                row.Cells[1].Value = $"{student.FirstName} {student.LastName}";
                row.Cells[2].Value = TranslateGenderEnum(student.Gender);
                row.Cells[3].Value = student.Email;
                row.Cells[4].Value = specialty.Name;
                row.Cells[5].Value = student.Semester;
                row.Cells[6].Value = student.StartDate;
                row.Cells[7].Value = student.EndDate;
                count++;
            }

            dgv_ListView.ClearSelection();
            btn_PagePrev.Enabled = _currentPage > 1;
            btn_PageNext.Enabled = count == _rowsPerPage;
        }

        private string TranslateGenderEnum(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Hombre",
                Gender.Female => "Mujer",
                Gender.Other => "Otro",
                _ => throw new UnreachableException(),
            };
        }
    }
}