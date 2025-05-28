using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Win10_Mobile_ToDoApp
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string _taskName;
        private bool _isCompleted;

        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BoolToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isCompleted && isCompleted)
                return TextDecorations.Strikethrough;
            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class ToDoPage : Page
    {
        private ObservableCollection<TaskItem> Tasks = new ObservableCollection<TaskItem>();

        public ToDoPage()
        {
            this.InitializeComponent();
            TaskListView.ItemsSource = Tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                Tasks.Add(new TaskItem { TaskName = TaskInput.Text, IsCompleted = false });
                TaskInput.Text = string.Empty;
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string taskName)
            {
                var task = Tasks.FirstOrDefault(t => t.TaskName == taskName);
                if (task != null)
                    Tasks.Remove(task);
            }
        }

        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RequestedTheme == ElementTheme.Light)
            {
                this.RequestedTheme = ElementTheme.Dark;
                ThemeToggleButton.Content = "Light Mode";
                this.Style = Resources["ContainerStyleDark"] as Style;
                TaskInput.Style = Resources["TextBoxStyleDark"] as Style;
                AddTaskButton.Style = Resources["AddButtonStyleDark"] as Style;
                ThemeToggleButton.Style = Resources["ThemeButtonStyleDark"] as Style;
                TaskListView.Style = Resources["ListViewStyleDark"] as Style;
                TaskListView.ItemContainerStyle = new Style { TargetType = typeof(ListViewItem), BasedOn = Resources["ListViewItemStyleDark"] as Style };
                foreach (var item in TaskListView.Items)
                {
                    if (TaskListView.ContainerFromItem(item) is ListViewItem listViewItem)
                    {
                        var textBlock = FindVisualChild<TextBlock>(listViewItem);
                        var deleteButton = FindVisualChild<Button>(listViewItem, "Delete");
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        if (textBlock != null)
                            textBlock.Style = Resources["TaskTextStyleDark"] as Style;
                        if (deleteButton != null)
                            deleteButton.Style = Resources["DeleteButtonStyleDark"] as Style;
                        if (checkBox != null)
                            checkBox.Style = Resources["CheckBoxStyleDark"] as Style;
                    }
                }
            }
            else
            {
                this.RequestedTheme = ElementTheme.Light;
                ThemeToggleButton.Content = "Dark Mode";
                this.Style = Resources["ContainerStyleLight"] as Style;
                TaskInput.Style = Resources["TextBoxStyleLight"] as Style;
                AddTaskButton.Style = Resources["AddButtonStyleLight"] as Style;
                ThemeToggleButton.Style = Resources["ThemeButtonStyleLight"] as Style;
                TaskListView.Style = Resources["ListViewStyleLight"] as Style;
                TaskListView.ItemContainerStyle = new Style { TargetType = typeof(ListViewItem), BasedOn = Resources["ListViewItemStyleLight"] as Style };
                foreach (var item in TaskListView.Items)
                {
                    if (TaskListView.ContainerFromItem(item) is ListViewItem listViewItem)
                    {
                        var textBlock = FindVisualChild<TextBlock>(listViewItem);
                        var deleteButton = FindVisualChild<Button>(listViewItem, "Delete");
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        if (textBlock != null)
                            textBlock.Style = Resources["TaskTextStyleLight"] as Style;
                        if (deleteButton != null)
                            deleteButton.Style = Resources["DeleteButtonStyleLight"] as Style;
                        if (checkBox != null)
                            checkBox.Style = Resources["CheckBoxStyleLight"] as Style;
                    }
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                task.IsCompleted = checkBox.IsChecked ?? false;
                if (TaskListView.ContainerFromItem(task) is ListViewItem listViewItem)
                {
                    var textBlock = FindVisualChild<TextBlock>(listViewItem);
                    if (textBlock != null)
                    {
                        textBlock.TextDecorations = task.IsCompleted ? TextDecorations.Strikethrough : TextDecorations.None;
                    }
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent, string name = null) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T target = null;
                if (child is T t && (name == null || (child is FrameworkElement fe && fe.Name == name)))
                {
                    target = t;
                    return target;
                }
                target = FindVisualChild<T>(child, name);
                if (target != null)
                    return target;
            }
            return null;
        }
    }
}