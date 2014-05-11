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
        private EventHandler<string> recognizedEvent;

        public bool Listening { get; set; }
        
        public SpeechEngine(EventHandler<string> recognizedEvent)
        {
            this.recognizedEvent = recognizedEvent;

            grammarManager = new GrammarManager();

            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(grammarManager.GetMenuGrammar());
            recognizer.LoadGrammar(grammarManager.GetSongGrammar());
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.SpeechRecognized += sr_SpeechRecognizedHandler;            

            recognizer.RecognizeCompleted += sr_RecognizeCompletedHandler;
            recognizer.SpeechDetected += sr_SpeechDetectedHandler;
            recognizer.SpeechHypothesized += sr_SpeechHypothesizedHandler;
            recognizer.SpeechRecognitionRejected += sr_SpeechRecognitionRejectedHandler;                        
        }

        public void Start()
        {            
            if (!Listening)
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Listening = true;
        }

        public void Stop()
        {
            recognizer.RecognizeAsyncCancel();
            
            Listening = false;
        }

        private void sr_SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text + " | " + e.Result.Confidence); 
            if (e.Result.Confidence < 0.85)
                return; 

            if (e.Result.Text.ToString() == "Stop Playing")
                Stop();

            recognizedEvent(this, e.Result.Text);           
        }


        private void sr_RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
            Console.WriteLine(e.Result.Text, e.Result.Confidence);
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
}
