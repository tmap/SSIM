using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class MemorySearchExample
{
    // Import the ReadProcessMemory function from kernel32.dll
    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        ref int lpNumberOfBytesRead
    );

    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: MemorySearchExample.exe <pid> <string to search>");
            return;
        }
        int pid = Convert.ToInt32(args[0]);
        string searchString = args[1];
        byte[] buffer;
        int bytesRead = 0;

        // Get the process by its ID
        Process process = Process.GetProcessById(pid);

        // Get the process's main module
        ProcessModule module = process.MainModule;

        // Allocate a buffer to hold the memory contents
        buffer = new byte[module.ModuleMemorySize];

        // Read the process's memory
        ReadProcessMemory(
            process.Handle,
            module.BaseAddress,
            buffer,
            buffer.Length,
            ref bytesRead
        );

        // Convert the memory contents to a string
        string memory = System.Text.Encoding.ASCII.GetString(buffer);

        // Check if the memory contains the search string
        int index = memory.IndexOf(searchString);
        if (index != -1)
        {
            Console.WriteLine("Found the string '{0}' in the memory of process {1}", searchString, pid);
            Console.WriteLine("First 1000 characters: " + memory.Substring(index, Math.Min(1000, memory.Length - index)));
        }
        else
        {
            Console.WriteLine("Could not find the string '{0}' in the memory of process {1}", searchString, pid);
        }
    }
}
