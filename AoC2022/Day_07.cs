namespace AdventOfCode
{
    public class Day_07 : BaseDay
    {
        class File
        {
            public string Name;
            public int Size;
        }

        class Folder
        {
            public string Name;
            public Folder Parent;
            public Dictionary<string, Folder> subfolders = new();
            public Dictionary<string, File> files = new();
            public int Size => size ??= 
                files.Values.Sum(x => x.Size) + subfolders.Values.Sum(x => x.Size); 
            private int? size;

            public IEnumerable<File> GetFiles()
            {
                return files.Values.Concat(subfolders.Values.SelectMany(x => x.GetFiles()));
            }

            public IEnumerable<Folder> GetFolders()
            {
                return subfolders.Values.Concat(subfolders.Values.SelectMany(x => x.GetFolders()));
            }
        }

        Folder ParseFolderStructure()
        {
            Folder rootFolder = new Folder(){Name="/"};

            Folder currentFolder = rootFolder;

            string[] commands = Data.Split("$", StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                var commandLines = command.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var commandLine = commandLines[0];
                var commandInput = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var commandName = commandInput[0];

                switch (commandName)
                {
                    case "cd":
                        string dir = commandInput[1];
                        if (dir == "..")
                            currentFolder = currentFolder.Parent;
                        else if (dir == "/")
                            currentFolder = rootFolder;
                        else
                            currentFolder = currentFolder.subfolders[dir];
                        break;
                    case "ls":
                        var commandOutputs = commandLines.Skip(1);
                        foreach (var commandOutput in commandOutputs)
                        {
                            // add results to the current folder
                            var fileinfo = commandOutput.Split(" ");
                            var filename = fileinfo[1];
                            if (fileinfo[0]=="dir")
                            {
                                currentFolder.subfolders.Add(filename, new Folder(){Name=filename, Parent = currentFolder});   
                            }
                            else
                            {
                                currentFolder.files.Add(filename, new File(){Name = filename, Size = int.Parse(fileinfo[0])});
                            }   
                        }
                        break;
                }
            }

            return rootFolder;
        }

        public override ValueTask<string> Solve_1()
        {
            var rootFolder = ParseFolderStructure();

            return new(rootFolder.GetFolders().Where(x => x.Size <= 100_000).Sum(x => x.Size).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var rootFolder = ParseFolderStructure();
            var toFree = 30_000_000 - (70_000_000 - rootFolder.Size);

            var result = rootFolder.GetFolders().Where(x => x.Size >= toFree).OrderBy(x => x.Size).First();
            return new(result.Size.ToString());
        }
    }
}
