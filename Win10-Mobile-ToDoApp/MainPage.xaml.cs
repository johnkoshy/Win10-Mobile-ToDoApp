using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.IO;
using Windows.Storage;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Win10MobileToDoApp
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<TaskItem> Tasks;
        private const string TasksFileName = "tasks.json";

        public MainPage()
        {
            this.InitializeComponent();
            LoadTasksAsync();
        }

        private async void LoadTasksAsync()
        {
            Tasks = new ObservableCollection<TaskItem>();
            try
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(TasksFileName) as StorageFile;
                if (file != null)
                {
                    var json = await FileIO.ReadTextAsync(file);
                    var taskList = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
                    foreach (var task in taskList)
                    {
                        Tasks.Add(task);
                    }
                }
            }
            catch (Exception)
            {
                // Handle file not found or invalid JSON silently
            }
            TaskListView.ItemsSource = Tasks;
        }

        private async void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                var newTask = new TaskItem
                {
                    Id = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1,
                    Description = TaskInput.Text
                };
                Tasks.Add(newTask);
                await SaveTasksAsync();
                TaskInput.Text = string.Empty;
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Please enter a task.");
                await dialog.ShowAsync();
            }
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int taskId = (int)button.Tag;
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                Tasks.Remove(task);
                await SaveTasksAsync();
            }
        }

        private async Task SaveTasksAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(TasksFileName, CreationCollisionOption.ReplaceExisting);
                var json = JsonConvert.SerializeObject(Tasks.ToList());
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog($"Error saving tasks: {ex.Message}");
                await dialog.ShowAsync();
            }
        }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}