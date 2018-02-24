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
using TabMaker.Models;
using NAudio.Wave;
namespace TabMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        Recorder testRecorder = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            testRecorder = new FileRecorder(@"./wav/test.wav", new WaveFormat(20000, 16, 1));
            testRecorder.StartRecording();
     
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (testRecorder != null)
            {
                testRecorder.StopRecording();
            }
            SpectrumAnalyser analyser = new SpectrumAnalyser(11);
                analyser.OpenFile(@"./wav/test.wav");
                analyser.AnalyseFile();
                var spectrogram = analyser.Spectrogram;
                var bitmap = spectrogram.DrawSpectogram();
                this.image.Source = bitmap;
                spectrogram.SaveToTxtFile(@"./txt/spectrogram.txt");
            
                
        }
    }
}
