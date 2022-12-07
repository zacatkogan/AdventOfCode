namespace AdventOfCode
{
    public class Day_07 : BaseDay
    {
        string sampleData = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

        class File
        {
            public int Size;
            public string Name;
        }

        class Folder
        {
            public string name;
            public Folder parent;
            public List<Folder> subfolders = new();
            public List<File> files = new();
            public int size => GetFiles().Sum(x => x.Size);

            public IEnumerable<File> GetFiles()
            {
                foreach (var file in files)
                    yield return file;
                
                foreach (var folder in subfolders)
                {
                    foreach (var subfile in folder.GetFiles())
                        yield return subfile;
                }
            }
            public IEnumerable<Folder> GetFolders()
            {
                foreach (var folder in subfolders)
                {
                    yield return folder;

                    foreach (var f in folder.GetFolders())
                    {
                        yield return f;
                    }
                }
            }
        }

        Folder ParseFolderStructure()
        {
            Folder rootFolder = new Folder(){name="/"};

            Folder currentFolder = rootFolder;

            // construct tree
            // cd means change to folder
            string[] lines = Data.Split("\n");

            foreach (var line in lines)
            {
                if (line.StartsWith("$"))
                {
                    var commandLine = line.Split(" ");
                    var command = commandLine[1];

                    switch (command)
                    {
                        case "cd":
                            string dir = commandLine[2];
                            if (dir == "..")
                                currentFolder = currentFolder.parent;
                            else if (dir == "/")
                                currentFolder = rootFolder;
                            else
                                currentFolder = currentFolder.subfolders.First(x => x.name == dir);
                            break;
                        case "ls":
                        break;
                    }
                }
                else
                {
                    // else we're part of an LS call.
                    // add results to the current folder
                    var fileinfo = line.Split(" ");
                    var filename = fileinfo[1];
                    if (fileinfo[0]=="dir")
                    {
                        var folder = currentFolder.subfolders.FirstOrDefault(x => x.name == filename);
                        if (folder != null)
                            continue;
                        
                        else 
                        {
                            folder = new Folder(){name=filename, parent = currentFolder};
                            currentFolder.subfolders.Add(folder);
                            
                        }
                    }
                    else
                    {
                        var file = currentFolder.files.FirstOrDefault(x => x.Name == filename);
                        if (file != null)
                            continue;
                        
                        currentFolder.files.Add(new File(){Name = filename, Size = int.Parse(fileinfo[0])});
                    }
                }
            }

            return rootFolder;
        }

        public override ValueTask<string> Solve_1()
        {
            var rootFolder = ParseFolderStructure();

            return new(rootFolder.GetFolders().Where(x => x.size <= 100_000).Sum(x => x.size).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var rootFolder = ParseFolderStructure();

            var filesystemTotalSize = 70000000;
            var totalUsed = rootFolder.size;
            var currentlyFree = filesystemTotalSize - totalUsed;
            var toFree = 30_000_000 - currentlyFree;

            var folderList = rootFolder.GetFolders().Where(x => x.size >= toFree).OrderBy(x => x.size).ToList();
            
            var result = folderList.First();
            
            return new(result.size.ToString());
        }
    }
}
