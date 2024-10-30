using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        private List<Task> tasks = new List<Task>();
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 60000; // Проверять каждую минуту
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void InitializeComboBox()
        {
            cboPriority.Items.Add("Низкий");
            cboPriority.Items.Add("Средний");
            cboPriority.Items.Add("Высокий");
            cboPriority.SelectedIndex = 0; // Установите значение по умолчанию
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            DateTime dueDate = dtpDueDate.Value;
            string priority = cboPriority.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(title))
            {
                Task newTask = new Task(title, dueDate, priority);
                tasks.Add(newTask);

                RefreshTaskList();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Введите название задачи.");
            }
        }

        private void RefreshTaskList()
        {
            listViewTasks.Items.Clear();
            foreach (var task in tasks)
            {
                ListViewItem item = new ListViewItem(task.Title);
                item.SubItems.Add(task.DueDate.ToString("d"));
                item.SubItems.Add(task.Priority);
                item.SubItems.Add(task.IsCompleted ? "Да" : "Нет");
                listViewTasks.Items.Add(item);
            }
        }

        private void ClearInputFields()
        {
            txtTaskTitle.Clear();
            dtpDueDate.Value = DateTime.Now;
            cboPriority.SelectedIndex = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedItems.Count > 0)
            {
                int index = listViewTasks.SelectedIndices[0];
                tasks.RemoveAt(index);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedItems.Count > 0)
            {
                int index = listViewTasks.SelectedIndices[0];
                Task selectedTask = tasks[index];

                txtTaskTitle.Text = selectedTask.Title;
                dtpDueDate.Value = selectedTask.DueDate;
                cboPriority.SelectedItem = selectedTask.Priority;

                tasks.RemoveAt(index);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования.");
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedItems.Count > 0)
            {
                int index = listViewTasks.SelectedIndices[0];
                tasks[index].IsCompleted = true;
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Выберите задачу для завершения.");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var task in tasks)
            {
                if (!task.IsCompleted && task.DueDate.Date == DateTime.Today)
                {
                    MessageBox.Show($"Срок выполнения задачи '{task.Title}' истекает сегодня!");
                }
            }
        }

        private void listViewTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class Task
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string title, DateTime dueDate, string priority)
        {
            Title = title;
            DueDate = dueDate;
            Priority = priority;
            IsCompleted = false;
        }
    }
}
