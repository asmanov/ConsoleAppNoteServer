using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConsoleAppNoteServer.Model
{
    public class Note : INotifyPropertyChanged
    {
        public Note()
        {
            Title = "Title";
            BodyNote = "Add Your Note";
            DateNote = DateTime.Now;
        }

        private int id;
        private string bodyNote;
        private string title;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                this.title = value;
                OnPropertyChanged("Title");
            }
        }

        public string BodyNote
        {
            get { return bodyNote; }
            set
            {
                bodyNote = value;
                OnPropertyChanged("BodyNote");
            }
        }

        private DateTime dateNote;

        public DateTime DateNote
        {
            get { return dateNote; }
            set
            {
                dateNote = value;
                OnPropertyChanged("DateNote");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
