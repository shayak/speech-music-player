using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpeechRecognition;
using SpeechRecognition.DataAccess;
using WinForms = System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SongRepo songRepo;
        private SpeechEngine speechEngine;
        private SongPlayer player;

        public event EventHandler<string> songRecognizedEvent;
        
        public MainWindow()
        {
            InitializeComponent();

            songRecognizedEvent += OnSongRecognized;

            songRepo = new SongRepo();
            speechEngine = new SpeechEngine(songRecognizedEvent);
            player = new SongPlayer();            
        }

        public void Add_Songs_Button_Click(Object sender, RoutedEventArgs e)
        {
            var dialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = dialog.ShowDialog();
            songRepo.AddSongsToDB(dialog.SelectedPath);
        }

        public void Start_Listening_Button_Click(Object sender, RoutedEventArgs e)
        {
            speechEngine.Start();
        }

        public void OnSongRecognized(object sender, string title)
        {
            player.PlaySong(songRepo, title);
        }
    }
}
