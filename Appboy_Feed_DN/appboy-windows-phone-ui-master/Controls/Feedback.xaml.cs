using AppboyPlatform.PCL.Utilities;
using AppboyPlatform.Phone;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppboyUI.Phone.Controls {
  public partial class Feedback : UserControl {
    public event EventHandler OnCancel;
    public event EventHandler AfterSubmit;

    public Feedback() {
      InitializeComponent();
      Loaded += Feedback_Loaded;
    }

    void Feedback_Loaded(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.EventLogger.LogFeedbackDisplayed();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) {
      if (OnCancel != null) {
        OnCancel(sender, null);
      }
    }

    private void Submit_Click(object sender, RoutedEventArgs e) {
      string message = MessageTextBox.Text.Trim();
      string email = EmailTextBox.Text.Trim();
      string warning = "";
      if (message.Length == 0) {
        warning = "A message is required.";        
      } else if (email.Length == 0) {
        warning = "An email address is required.";        
      } else if (!Validations.IsValidEmailAddress(email)) {
        warning = "The email address is not valid.";
      }

      if (warning.Length > 0) {
        MessageBox.Show(warning);
        return;
      }

      Appboy.SharedInstance.SubmitFeedback(email, message, ReportingIssueCheckBox.IsChecked ?? false);
      if (AfterSubmit != null) {
        AfterSubmit(sender, null);
      }
    }
  }
}
