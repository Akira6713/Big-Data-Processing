/// <summary>
///  * Author: Eric Ramirez
///  * Created: 06/2024
/// </summary>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

class HugeFileProcessor
{
    private const int ChunkSize = 100000; // Define the number of lines to read in each chunk from the file.

    // A thread-safe collection for storing chunks of lines from two files for processing.
    private BlockingCollection<(string[], string[])> chunksToProcess = new BlockingCollection<(string[], string[])>();

    // Initiates concurrent file reading and processing tasks, and outputs the results to a specified file.
    public void ProcessFiles(string file1Path, string file2Path, string outputPath)

    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        var readTask1 = Task.Run(() => ReadFileInChunks(file1Path, 1));
        var readTask2 = Task.Run(() => ReadFileInChunks(file2Path, 2));
        var processAndWriteTask = Task.Run(() => ProcessChunksAndWrite(outputPath));

        Task.WaitAll(readTask1, readTask2);

        chunksToProcess.CompleteAdding();

        processAndWriteTask.Wait();

        watch.Stop();
        Console.WriteLine($"Processing completed in {watch.ElapsedMilliseconds} ms.");
    }

    // Reads a file in chunks and adds them to the processing queue.
    private void ReadFileInChunks(string filePath, int fileNumber)
    {
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var lines = new string[ChunkSize];
                for (int i = 0; i < ChunkSize && !reader.EndOfStream; i++)
                {
                    lines[i] = reader.ReadLine();
                }
                if (fileNumber == 1)
                {
                    chunksToProcess.Add((lines, null));
                }
                else
                {
                    var existingChunk = chunksToProcess.Take();
                    chunksToProcess.Add((existingChunk.Item1, lines));
                }
            }
        }
    }

    // Processes paired chunks from two files and writes the result to an output file.
    private void ProcessChunksAndWrite(string outputPath)
    {
        using (var writer = new StreamWriter(outputPath))
        {
            int lineNumber = 0; // Keep track of line number for better error reporting
            foreach (var chunk in chunksToProcess.GetConsumingEnumerable())
            {
                if (chunk.Item1 != null && chunk.Item2 != null)
                {
                    for (int i = 0; i < ChunkSize && chunk.Item1[i] != null && chunk.Item2[i] != null; i++)
                    {
                        lineNumber++;
                        var trimmedLine1 = chunk.Item1[i].Trim();
                        var trimmedLine2 = chunk.Item2[i].Trim();

                        // Treat empty strings as "0"
                        int number1 = string.IsNullOrEmpty(trimmedLine1) ? 0 : int.Parse(trimmedLine1);
                        int number2 = string.IsNullOrEmpty(trimmedLine2) ? 0 : int.Parse(trimmedLine2);

                        var sum = number1 + number2;
                        writer.WriteLine(sum);
                    }
                }
            }
        }
    }




}

class Program
{
    static void Main(string[] args)
    {
        var processor = new HugeFileProcessor();
        processor.ProcessFiles("hugefile1.txt", "hugefile2.txt", "totalfile.txt");
    }
}
