# BigDataProcessing

## Description
HugeFileProcessor is a C# application designed to efficiently process large numerical data files. It reads two large data files concurrently, sums corresponding numerical values from each file, and writes the results to a new output file. This application leverages multithreading and parallel processing techniques to optimize performance, particularly beneficial for tasks limited by I/O and CPU throughput.

## Key Features
- **Concurrent File Reading:** Utilizes asynchronous tasks to read two files in parallel, significantly reducing I/O time.
- **Data Chunking:** Handles data in manageable chunks to ensure a low memory footprint and enhance processing speed.
- **Thread Safety:** Employs a `BlockingCollection` to synchronize data between reading and processing tasks, ensuring data integrity and thread safety.
- **Performance Metrics:** Measures and reports the processing time to provide insights into the application’s efficiency.
- **Error Handling:** Includes basic error handling for file operations and data parsing.

## Usage
To use the HugeFileProcessor, follow these steps:

1. **Setup Environment:**
   Ensure that you have a C# development environment set up, such as Visual Studio or .NET Core CLI.

2. **Clone the Repository:**

git clone https://github.com/Akira6713/HugeFileProcessor.git

3. **Navigate to the Project Directory:**

cd HugeFileProcessor

4. **Compile the Application:**
If you are using the .NET Core CLI, you can compile with the following command:

dotnet run -- hugefile1.txt hugefile2.txt totalfile.txt


## Additional Notes
- This application is optimized for processing numerical data. Adjustments may be needed for other data types.
- Future enhancements could include more sophisticated error handling and support for additional file formats.

## Author
- Eric Ramirez

