using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

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

                if (Resources.TryGetValue("ContainerStyleDark", out var containerStyle) && containerStyle is Style)
                    this.Style = (Style)containerStyle;
                if (Resources.TryGetValue("TextBoxStyleDark", out var textBoxStyle) && textBoxStyle is Style)
                    TaskInput.Style = (Style)textBoxStyle;
                if (Resources.TryGetValue("AddButtonStyleDark", out var addButtonStyle) && addButtonStyle is Style)
                    AddTaskButton.Style = (Style)addButtonStyle;
                if (Resources.TryGetValue("ThemeButtonStyleDark", out var themeButtonStyle) && themeButtonStyle is Style)
                    ThemeToggleButton.Style = (Style)themeButtonStyle;
                if (Resources.TryGetValue("ListViewStyleDark", out var listViewStyle) && listViewStyle is Style)
                    TaskListView.Style = (Style)listViewStyle;
                if (Resources.TryGetValue("ListViewItemStyleDark", out var listViewItemStyle) && listViewItemStyle is Style)
                    TaskListView.ItemContainerStyle = new Style { TargetType = typeof(ListViewItem), BasedOn = (Style)listViewItemStyle };

                foreach (var item in TaskListView.Items)
                {
                    if (TaskListView.ContainerFromItem(item) is ListViewItem listViewItem)
                    {
                        var textBlock = FindVisualChild<TextBlock>(listViewItem);
                        var deleteButton = FindVisualChild<Button>(listViewItem, "Delete");
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        if (textBlock != null)
                        {
                            if (Resources.TryGetValue("TaskTextStyleDark", out var textStyle) && textStyle is Style)
                                textBlock.Style = (Style)textStyle;
                            textBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.White); // Fallback
                            System.Diagnostics.Debug.WriteLine($"Applied TaskTextStyleDark to TextBlock: {textBlock.Text}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to find TextBlock in ListViewItem");
                        }
                        if (deleteButton != null && Resources.TryGetValue("DeleteButtonStyleDark", out var deleteStyle) && deleteStyle is Style)
                            deleteButton.Style = (Style)deleteStyle;
                        if (checkBox != null && Resources.TryGetValue("CheckBoxStyleDark", out var checkBoxStyle) && checkBoxStyle is Style)
                            checkBox.Style = (Style)checkBoxStyle;
                        listViewItem.PointerEntered += (s, args) => StartHoverAnimation(listViewItem, true);
                        listViewItem.PointerExited += (s, args) => ResetBackground(listViewItem, true);
                    }
                }
            }
            else
            {
                this.RequestedTheme = ElementTheme.Light;
                ThemeToggleButton.Content = "Dark Mode";

                if (Resources.TryGetValue("ContainerStyleLight", out var containerStyle) && containerStyle is Style)
                    this.Style = (Style)containerStyle;
                if (Resources.TryGetValue("TextBoxStyleLight", out var textBoxStyle) && textBoxStyle is Style)
                    TaskInput.Style = (Style)textBoxStyle;
                if (Resources.TryGetValue("AddButtonStyleLight", out var addButtonStyle) && addButtonStyle is Style)
                    AddTaskButton.Style = (Style)addButtonStyle;
                if (Resources.TryGetValue("ThemeButtonStyleLight", out var themeButtonStyle) && themeButtonStyle is Style)
                    ThemeToggleButton.Style = (Style)themeButtonStyle;
                if (Resources.TryGetValue("ListViewStyleLight", out var listViewStyle) && listViewStyle is Style)
                    TaskListView.Style = (Style)listViewStyle;
                if (Resources.TryGetValue("ListViewItemStyleLight", out var listViewItemStyle) && listViewItemStyle is Style)
                    TaskListView.ItemContainerStyle = new Style { TargetType = typeof(ListViewItem), BasedOn = (Style)listViewItemStyle };

                foreach (var item in TaskListView.Items)
                {
                    if (TaskListView.ContainerFromItem(item) is ListViewItem listViewItem)
                    {
                        var textBlock = FindVisualChild<TextBlock>(listViewItem);
                        var deleteButton = FindVisualChild<Button>(listViewItem, "Delete");
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        if (textBlock != null)
                        {
                            if (Resources.TryGetValue("TaskTextStyleLight", out var textStyle) && textStyle is Style)
                                textBlock.Style = (Style)textStyle;
                            textBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 51, 51, 51)); // Fallback
                            System.Diagnostics.Debug.WriteLine($"Applied TaskTextStyleLight to TextBlock: {textBlock.Text}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to find TextBlock in ListViewItem");
                        }
                        if (deleteButton != null && Resources.TryGetValue("DeleteButtonStyleLight", out var deleteStyle) && deleteStyle is Style)
                            deleteButton.Style = (Style)deleteStyle;
                        if (checkBox != null && Resources.TryGetValue("CheckBoxStyleLight", out var checkBoxStyle) && checkBoxStyle is Style)
                            checkBox.Style = (Style)checkBoxStyle;
                        listViewItem.PointerEntered += (s, args) => StartHoverAnimation(listViewItem, false);
                        listViewItem.PointerExited += (s, args) => ResetBackground(listViewItem, false);
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

        private void Button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button && button.RenderTransform is CompositeTransform transform)
            {
                System.Diagnostics.Debug.WriteLine($"Button pressed: {button.Content}");
                var storyboard = new Storyboard();
                var scaleX = new DoubleAnimation { To = 0.9, Duration = TimeSpan.FromMilliseconds(100) };
                var scaleY = new DoubleAnimation { To = 0.9, Duration = TimeSpan.FromMilliseconds(100) };
                var opacity = new DoubleAnimation { To = 0.7, Duration = TimeSpan.FromMilliseconds(100) };
                Storyboard.SetTarget(scaleX, transform);
                Storyboard.SetTarget(scaleY, transform);
                Storyboard.SetTarget(opacity, button);
                Storyboard.SetTargetProperty(scaleX, "ScaleX");
                Storyboard.SetTargetProperty(scaleY, "ScaleY");
                Storyboard.SetTargetProperty(opacity, "Opacity");
                storyboard.Children.Add(scaleX);
                storyboard.Children.Add(scaleY);
                storyboard.Children.Add(opacity);
                storyboard.Begin();
            }
        }

        private void Button_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button && button.RenderTransform is CompositeTransform transform)
            {
                var storyboard = new Storyboard();
                var scaleX = new DoubleAnimation { To = 1.0, Duration = TimeSpan.FromMilliseconds(100) };
                var scaleY = new DoubleAnimation { To = 1.0, Duration = TimeSpan.FromMilliseconds(100) };
                var opacity = new DoubleAnimation { To = 1.0, Duration = TimeSpan.FromMilliseconds(100) };
                Storyboard.SetTarget(scaleX, transform);
                Storyboard.SetTarget(scaleY, transform);
                Storyboard.SetTarget(opacity, button);
                Storyboard.SetTargetProperty(scaleX, "ScaleX");
                Storyboard.SetTargetProperty(scaleY, "ScaleY");
                Storyboard.SetTargetProperty(opacity, "Opacity");
                storyboard.Children.Add(scaleX);
                storyboard.Children.Add(scaleY);
                storyboard.Children.Add(opacity);
                storyboard.Begin();
            }
        }

        private void StartHoverAnimation(ListViewItem item, bool isDarkTheme)
        {
            var storyboard = isDarkTheme ? Resources["HoverAnimationDark"] as Storyboard : Resources["HoverAnimation"] as Storyboard;
            Storyboard.SetTarget(storyboard, item);
            storyboard?.Begin();
        }

        private void ResetBackground(ListViewItem item, bool isDarkTheme)
        {
            item.Background = isDarkTheme
                ? new LinearGradientBrush
                {
                    StartPoint = new Windows.Foundation.Point(0, 0),
                    EndPoint = new Windows.Foundation.Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Windows.UI.Color.FromArgb(255, 51, 51, 51), Offset = 0 },
                        new GradientStop { Color = Windows.UI.Color.FromArgb(255, 44, 44, 44), Offset = 1 }
                    }
                }
                : new LinearGradientBrush
                {
                    StartPoint = new Windows.Foundation.Point(0, 0),
                    EndPoint = new Windows.Foundation.Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Windows.UI.Colors.White, Offset = 0 },
                        new GradientStop { Color = Windows.UI.Color.FromArgb(255, 245, 245, 245), Offset = 1 }
                    }
                };
            item.Opacity = 1.0;
            if (item.RenderTransform is CompositeTransform transform)
            {
                transform.TranslateY = 0;
            }
        }

        private T FindVisualChild<T>(DependencyObject parent, string name = null) where T : class
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // Check if child is of type T
                if (typeof(T).IsAssignableFrom(child.GetType()))
                {
                    // If name is specified, check FrameworkElement name
                    if (name != null && child is FrameworkElement fe && fe.Name == name)
                    {
                        return child as T;
                    }
                    // If no name specified, return the child
                    else if (name == null)
                    {
                        return child as T;
                    }
                }

                // Recursively search children
                T result = FindVisualChild<T>(child, name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}