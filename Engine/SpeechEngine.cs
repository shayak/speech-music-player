using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Recognition;
using System.IO;

namespace SpeechRecognition
{
    public class SpeechEngine
    {
        private SpeechRecognitionEngine recognizer;
        private GrammarManager grammarManager;
        private EventHandler<SpeechRecognizedUpdateArgs> recognizedEvent;

        public bool Listening { get; set; }
        
        public SpeechEngine(EventHandler<SpeechRecognizedUpdateArgs> recognizedEvent)
        {
            this.recognizedEvent = recognizedEvent;

            grammarManager = new GrammarManager();

            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(grammarManager.GetMenuGrammar());

            loadGrammarFromDB();
            
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.SpeechRecognized += sr_SpeechRecognizedHandler;            

            recognizer.RecognizeCompleted += sr_RecognizeCompletedHandler;
            recognizer.SpeechDetected += sr_SpeechDetectedHandler;
            recognizer.SpeechHypothesized += sr_SpeechHypothesizedHandler;
            recognizer.SpeechRecognitionRejected += sr_SpeechRecognitionRejectedHandler;                        
        }

        public void Start()
        {
            loadGrammarFromDB();
            if (!Listening)
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Listening = true;            
        }

        public void Stop()
        {
            recognizer.RecognizeAsyncCancel();
            
            Listening = false;
        }

        private void loadGrammarFromDB()
        {
            var gram = grammarManager.GetSongGrammar();
            if (gram != null)
                recognizer.LoadGrammar(gram);
        }

        private void sr_SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text + " | " + e.Result.Confidence);

            if (e.Result.Text.ToString() == "Stop Listening")
                Stop();

            recognizedEvent(this, new SpeechRecognizedUpdateArgs(e.Result.Text, e.Result.Confidence));
        }


        private void sr_RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
            
        }

        private void sr_SpeechDetectedHandler(object sender, SpeechDetectedEventArgs e)
        {
            
        }

        private void sr_SpeechHypothesizedHandler(object sender, SpeechHypothesizedEventArgs e)
        {

        }

        private void sr_SpeechRecognitionRejectedHandler(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }
    }

    public class SpeechRecognizedUpdateArgs : EventArgs
    {
        public string title;
        public double confidence;

        public SpeechRecognizedUpdateArgs(string title, double confidence)
        {
            this.title = title;
            this.confidence = confidence;
        }
    }
}
