using System;
using System.IO;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace InputSynthesizer
{
    public class InputSynthesizer
    {
        private string keyboardInputFilePath = Environment.GetEnvironmentVariable("KEYBOARD_INPUT_FILE");
        private string voiceRecognitionCulture = Environment.GetEnvironmentVariable("VOICE_RECOGNITION_CULTURE");

        public InputSynthesizer()
        {
            try
            {
                InitializeSpeechRecognition();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize speech recognition: {ex.Message}");
            }
        }

        private void InitializeSpeechRecognition()
        {
            try
            {
                if (string.IsNullOrEmpty(voiceRecognitionCulture))
                {
                    throw new InvalidOperationException("Voice recognition culture environment variable is not set.");
                }

                SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new CultureInfo(voiceRecognitionCulture));
                recognizer.LoadGrammar(new DictationGrammar());
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing speech recognition: {ex.Message}");
                throw; // Re-throw to be captured by the constructor
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Voice input recognized: " + e.Result.Text);
            ProcessSpeechInput(e.Result.Text);
        }

        private void ProcessSpeechInput(string input)
        {
            try
            {
                if (input.ToLower().Contains("open notepad"))
                {
                    Process.Start("notepad.exe");
                    Console.WriteLine("Opened notepad!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process speech input: {ex.Message}");
            }
        }

        public void ProcessKeyboardInput()
        {
            try
            {
                if (string.IsNullOrEmpty(keyboardInputFilePath))
                {
                    throw new InvalidOperationException("Keyboard input file path environment variable is not set.");
                }

                if (!File.Exists(keyboardInputFilePath))
                {
                    Console.WriteLine("Keyboard input file not found.");
                    return;
                }

                string text = File.ReadAllText(keyboardInputFilePath);
                Console.WriteLine("Keyboard input read from file: " + text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading keyboard input file: {ex.Message}");
            }
        }

        public void ProcessMouseInput()
        {
            Console.WriteLine("Mouse inputs would typically be captured using a global hook.");
        }

        static void Main(string[] args)
        {
            try
            {
                InputSynthesizer synthesizer = new InputSynthesizer();
                synthesizer.ProcessKeyboardInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}