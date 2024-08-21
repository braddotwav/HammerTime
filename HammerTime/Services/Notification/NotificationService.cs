using System.Media;
using Notification.Wpf;
using System.Windows.Media;

namespace HammerTime.Services.Notification;

internal class NotificationService
{
    private readonly NotificationManager notificationManager = new();
    private readonly SoundPlayer notifSound = new("Assets/Audio/notification.wav");

    public void Push(string title, string content, bool playSound = true)
    {
        var notificationContent = new NotificationContent()
        {
            Title = title,
            Message = content,
            Type = NotificationType.None,
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23bb33")),
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECF0ED")),
        };

        notificationManager.Show(notificationContent, ShowXbtn: false);

        try
        {
            if (!playSound) return;
            notifSound.Play();
        }
        catch (Exception)
        {
        }
    }
}
