using Aula4;

class Program
{
    static void Main(string[] args)
    {
        #region Manipulate Files

        var file = FileManager.CreateFile("teste.txt");

        var fileName = file.Name;

        FileManager.WriteOnFile(fileName, "Olá Mundo");
        FileManager.WriteOnFile(fileName, "Oi");

        FileManager.ReadFileContent(fileName);

        FileManager.ClearFile(fileName);

        FileManager.DeleteFile(fileName);

        #endregion

        Console.WriteLine();

        #region Manipulate Audios

        const string fileAudioName = "output.wav";

        Audio.AudiosCsCore();
        Audio.RecordVoiceCsCore(fileAudioName);
        Audio.HearVoiceCsCore(fileAudioName);

        //Usando biblioteca padrão
        Audio.HearVoice(fileAudioName);

        #endregion
    }      
}