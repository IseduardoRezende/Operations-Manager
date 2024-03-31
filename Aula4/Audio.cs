using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using System.Media;

namespace Aula4
{
    public static class Audio
    {
        private static bool IsValidFile(string? fileName)
        {
            if  (string.IsNullOrWhiteSpace(fileName)) 
                return false;       
        
            if (!fileName.Contains(".wav"))
                return false;

            return true;
        }

        public static void HearVoice(string fileName)
        {
            if (!IsValidFile(fileName))
                throw new ArgumentException("Invalid file name - (type must be .wav or file is null/empty)");

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            Console.WriteLine("Listando...");

            //Essa classe funciona apenas em Windows (leitor de .wav)
            var soundPlayer = new SoundPlayer(fileName);
            soundPlayer.PlaySync();
        }

        public static void HearVoiceCsCore(string fileName)
        {
            if (!IsValidFile(fileName))
                throw new ArgumentException("Invalid file name - (type must be .wav or file is null/empty)");

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            Console.WriteLine("Listando...");

            // Inicializa o WasapiOut
            using var capture = new WasapiOut();

            // Inicializa o WaveFileReader para ler o áudio de um arquivo .wav
            using var reader = new WaveFileReader(fileName);

            // Inicializa o WasapiOut e começa a tocar o áudio
            capture.Initialize(reader);

            capture.Play();

            while (capture.PlaybackState != PlaybackState.Stopped) { }

            Console.WriteLine();

            // Finaliza toque do áudio
            capture.Stop();
        }

        public static void RecordVoiceCsCore(string fileName)
        {
            if (!IsValidFile(fileName))
                throw new ArgumentException("Invalid file name - (type must be .wav or file is null/empty)");
            
            Console.WriteLine("Vai começar...");

            Thread.Sleep(1500);

            Console.WriteLine("Gravando!");

            // Inicializa o WasapiCapture
            using var capture = new WasapiCapture();
            capture.Initialize();

            // Captura o dispositivo de áudio que desejo (por padrão quero o 1°)
            capture.Device = new MMDeviceEnumerator().EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active).First();

            // Inicializa o SoundInSource com o WasapiCapture
            using var waveSource = new SoundInSource(capture);

            // Inicializa o WaveWriter para gravar o áudio em um arquivo .wav
            using var writer = new WaveWriter(fileName, waveSource.WaveFormat);

            // Registra um evento para escrever os dados de áudio capturados pelo WasapiCapture no WaveWriter
            capture.DataAvailable += (s, e) => writer.Write(e.Data, e.Offset, e.ByteCount);

            // Começa a captura de áudio
            capture.Start();

            Console.WriteLine("Pressione '1' para parar...");

            // Aguarda até que o usuário pressione '1' para parar a gravação
            while (Console.ReadKey().KeyChar != '1') { }

            Console.WriteLine();

            //Encerra captura de áudio
            capture.Stop();
        }

        public static void AudiosCsCore()
        {
            Console.WriteLine("Dispositivos de Áudio:");

            // Obtém o enumerador de dispositivos de áudio
            var enumerator = new MMDeviceEnumerator();

            // Obtém todos os dispositivos de áudio de saída
            var devices = enumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.All);

            // Exibe os dispositivos de áudio de saída disponíveis
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {devices[i].FriendlyName}");
            }

            // Solicita ao usuário selecionar o dispositivo de áudio de saída desejado
            Console.Write("Selecione o número do dispositivo de áudio de saída: ");
            var selectedDeviceIndex = int.Parse(Console.ReadLine()!) - 1;

            // Verifica se o índice selecionado é válido
            if (selectedDeviceIndex > -1 && selectedDeviceIndex < devices.Count)
            {
                // Obtém o dispositivo de áudio selecionado
                var selectedDevice = devices[selectedDeviceIndex];

                Console.WriteLine($"Dispositivo de áudio: \"{selectedDevice.FriendlyName}\" | ID - {selectedDevice.DeviceID}");
            }
            else
            {
                Console.WriteLine("Índice inválido.");
            }
        }
    }
}
