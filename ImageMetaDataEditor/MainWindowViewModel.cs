using JpgImage;
using Microsoft.Win32;
using MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageMetaDataEditor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? control = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(control));
        }


        private string _comSegmentText;
        public string ComSegmentText
        {
            get => _comSegmentText;
            set
            {
                _comSegmentText = value; 
                OnPropertyChanged(nameof(ComSegmentText));
            }
        }


        private RelayCommand _loadJpgFileCommand;
        public RelayCommand LoadJpgFileCommand => _loadJpgFileCommand ?? (_loadJpgFileCommand = new RelayCommand(param => LoadJpgFile()));
        private void LoadJpgFile()
        {
            ReadJpgFile readJpgFile = new ReadJpgFile();
            IfdData readExifData = new IfdData();
            List<IfdData> exifIfdData = new List<IfdData>();

            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg",
                Title = "Open JPEG File"
            };

            if(openFileDialog.ShowDialog() == false)
                return;

            List<byte[]> segments = readJpgFile.GetSegments(openFileDialog.FileName);

            ComSegmentText = "";
            string comSegment = "";
            foreach (byte[] segment in segments)
            {
                // Checking if COM segment.
                if (segment[1] == 0xFE)
                {
                    comSegment += 
                        $"""

                        -----------------------------------------
                        Segment Type = Comment
                        -----------------------------------------
                        {Encoding.ASCII.GetString(segment)}\r\n

                        """;
                }

                // Checking if APPn segment
                else if (segment[1] >= 0xE0 && segment[1] <= 0xEF)
                {
                    comSegment +=
                        $"""

                        -----------------------------------------
                        Segment Type = {JpgDictionaries.DictionaryOfMarkers[segment[1]]}
                        Tag = {Encoding.ASCII.GetString(new byte[] { segment[4], segment[5], segment[6], segment[7], segment[8] })}
                        Length = {segment.Length}
                        -----------------------------------------
                        """;

                    List<IfdData> listOfIfds = readJpgFile.GetMetaData(segment);
                    if(listOfIfds.Count > 0)
                    {
                        foreach (IfdData data in listOfIfds)
                        {
                            exifIfdData.Add(data);
                            comSegment += $"""
                                          
                                          {data.DataType}: {data.DataValue}
                                          """;
                        }
                    }
                }
            }

            ComSegmentText = comSegment.Trim();
        }
    }
}