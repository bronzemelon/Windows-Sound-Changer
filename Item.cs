using System.ComponentModel;

namespace Windows_Sound_Changer
{
    /// <summary>
    /// Represents a program event in a ListBox
    /// </summary>
    public class Item : INotifyPropertyChanged
    {
        public string Name { get; set; } = string.Empty;
        public string KeyName { get; set; } = string.Empty;
        public bool IsModified { get; set; } = false;

        private string _buttonText = "Choose Sound";
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}