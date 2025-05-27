using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Win10_Mobile_ToDoApp
{
    public class ToDoItem
    {
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }

    public sealed partial class ToDoPage : Page
    {
        private ObservableCollection<ToDoItem> Tasks = new ObservableCollection<ToDoItem>();

        public ToDoPage()
        {
            this.InitializeComponent();
            TaskListView.ItemsSource = Tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                Tasks.Add(new ToDoItem { TaskName = TaskInput.Text, IsCompleted = false });
                TaskInput.Text = string.Empty;
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string taskName)
            {
                var taskToRemove = Tasks.FirstOrDefault(t => t.TaskName == taskName);
                if (taskToRemove != null)
                {
                    Tasks.Remove(taskToRemove);
                }
            }
        }
    }
}