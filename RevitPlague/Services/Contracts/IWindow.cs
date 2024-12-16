namespace RevitPlague.Services.Contracts;

public interface IWindow
{
    void Close();
    void Show();
    bool? ShowDialog();
}