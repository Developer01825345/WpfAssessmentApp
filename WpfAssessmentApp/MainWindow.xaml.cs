using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfAssessmentApp.Data;
using WpfAssessmentApp.Model;

namespace WpfAssessmentApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly UserRepository _userRepository;
    private User _selectedUser;
    private bool _isEnabled = false;

    public MainWindow()
    {
        InitializeComponent();
        _userRepository = new UserRepository();
        GetAllUsers();
        DOBDatePicker.DisplayDateEnd = DateTime.Today;
        SetErrorOnLoad();
    }

    private void GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        UserDataGrid.ItemsSource = users;
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var user = new User
        {
            FirstName = FirstNameTextBox.Text,
            LastName = LastNameTextBox.Text,
            DOB = DOBDatePicker.SelectedDate.Value,
            Email = EmailTextBox.Text,
            Gender = MaleRadioButton.IsChecked == true ? "Male" : "Female",
            Phone = PhoneTextBox.Text,
            Language = ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString()
        };

        _userRepository.AddUser(user);
        GetAllUsers();
        ClearForm();
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedUser = (User)UserDataGrid.SelectedItem;

        selectedUser.FirstName = FirstNameTextBox.Text;
        selectedUser.LastName = LastNameTextBox.Text;
        selectedUser.DOB = DOBDatePicker.SelectedDate.Value;
        selectedUser.Email = EmailTextBox.Text;
        selectedUser.Gender = MaleRadioButton.IsChecked == true ? "Male" : "Female";
        selectedUser.Phone = PhoneTextBox.Text;
        selectedUser.Language = ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString();

        _userRepository.UpdateUser(selectedUser);
        GetAllUsers();
        ClearForm();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedUser = (User)UserDataGrid.SelectedItem;

        var result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            _userRepository.DeleteUser(selectedUser.Id);
            GetAllUsers();
            ClearForm();
        }
    }

    private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UserDataGrid.SelectedItem is User user)
        {
            _selectedUser = user;
            FirstNameTextBox.Text = user.FirstName;
            LastNameTextBox.Text = user.LastName;
            DOBDatePicker.SelectedDate = user.DOB;
            EmailTextBox.Text = user.Email;
            PhoneTextBox.Text = user.Phone;
            
            if (user.Gender == "Male")
            {
                MaleRadioButton.IsChecked = true;
            }
            else
            {
                FemaleRadioButton.IsChecked = true;
            }

            foreach (ComboBoxItem item in LanguageComboBox.Items)
            {
                if (item.Content.ToString() == user.Language)
                {
                    LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }

    private void ClearForm()
    {
        FirstNameTextBox.Clear();
        LastNameTextBox.Clear();
        DOBDatePicker.SelectedDate = null;
        EmailTextBox.Clear();
        PhoneTextBox.Clear();
        MaleRadioButton.IsChecked = true;
        LanguageComboBox.SelectedIndex = 0;
    }

    private void SetErrorOnLoad()
    {
        FirstNameTextBox.Background = Brushes.Red;
        LastNameTextBox.Background = Brushes.Red;
        DOBDatePicker.Background = Brushes.Red;
        EmailTextBox.Background = Brushes.Red;
        PhoneTextBox.Background = Brushes.Red;
        LanguageComboBox.Background = Brushes.Red;
    }

    private void SetError(Control control)
    {
        control.Background = Brushes.Red;
        _isEnabled = false;
    }

    private void RemoveError(Control control)
    {
        control.Background = Brushes.White;
        _isEnabled = true;
    }

    private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetError(FirstNameTextBox);
        if (!string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
        {
            RemoveError(FirstNameTextBox);
        }
    }

    private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetError(LastNameTextBox);
        if (!string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            RemoveError(LastNameTextBox);
    }

    private void DOBTextBox_TextChanged(object sender, SelectionChangedEventArgs e)
    {
        SetError(DOBDatePicker);
        if (DOBDatePicker.SelectedDate.HasValue)
        {
            if (DOBDatePicker.SelectedDate is DateTime dob)
            {
                int age = CalculateAge(dob);
                 if (age >= 18)
                {
                    MessageBox.Show("error");
                    //DOBDatePicker.Background = Brushes.White;
                    //SubmitButton.IsEnabled = true;
                    RemoveError(DOBDatePicker); // Optional, if you're showing visual error cues
                }
            }
        }
    }

    private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetError(EmailTextBox);
        if (!string.IsNullOrWhiteSpace(EmailTextBox.Text) && Regex.IsMatch(EmailTextBox.Text, @"^[^@]+@[^@]+\.[^@]+$"))
            RemoveError(EmailTextBox);
    }

    private void Gender_Checked(object sender, RoutedEventArgs e)
    {
        SetError(MaleRadioButton);
        if (MaleRadioButton.IsChecked == true)
            RemoveError(MaleRadioButton); // Handle the valid case
    }
    private void Phone_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetError(PhoneTextBox);
        if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text) && Regex.IsMatch(PhoneTextBox.Text, @"^\(\d{3}\) \d{3}-\d{4}$"))
            RemoveError(PhoneTextBox);
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetError(LanguageComboBox);
        if (LanguageComboBox.SelectedItem != null)
            RemoveError(LanguageComboBox);
    }

    private void ValidateSubmitButton()
    {
        AddButton.IsEnabled = _isEnabled;
    }
    private int CalculateAge(DateTime dob)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-age)) age--;

        return age;
    }
}