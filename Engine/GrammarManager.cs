using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.IO;
using System.Data;
using SpeechRecognition.DataAccess;

namespace SpeechRecognition
{
    public class GrammarManager
    {
        private SongRepo songRepo;
        private string path;
        
        public GrammarManager()
        {        
            songRepo = new SongRepo();
        }

        public GrammarManager(SongRepo repo)
        {
            this.songRepo = repo;
        }

        public Grammar GetSongGrammar()
        {
            Choices songs = new Choices();
            songs.Add(songRepo.GetAllTitles());
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(songs);
            return new Grammar(gb);
        }

        public Grammar GetMenuGrammar()
        {
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(new Choices("Play a song", "Stop Playing"));            
            return new Grammar(gb);
        }       
    }
}
