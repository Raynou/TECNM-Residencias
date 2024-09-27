namespace TECNM.Residencias.Services;

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents a service for creating compressed backups of the storage system in a specific directory.
/// </summary>
internal sealed class StorageBackupService
{
    private readonly string sourceDirectory;
    private readonly string destinationDirectory;
    private readonly DateTime backupDateTime;
    private IList<string> preparedFiles = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageBackupService"/> class
    /// with a specified source directory, destination directory, and backup date/time.
    /// </summary>
    /// <param name="sourceDirectory">The directory containing files to be backed up.</param>
    /// <param name="destinationDirectory">The directory where the backup will be saved.</param>
    /// <param name="backupDateTime">The date and time of the backup.</param>
    public StorageBackupService(string sourceDirectory, string destinationDirectory, DateTime backupDateTime)
    {
        this.sourceDirectory = sourceDirectory;
        this.destinationDirectory = destinationDirectory;
        this.backupDateTime = backupDateTime;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageBackupService"/> class
    /// with a specified source directory and destination directory. The current date/time
    /// is used as the backup time.
    /// </summary>
    /// <param name="sourceDirectory">The directory containing files to be backed up.</param>
    /// <param name="destinationDirectory">The directory where the backup will be saved.</param>
    public StorageBackupService(string sourceDirectory, string destinationDirectory)
        : this(sourceDirectory, destinationDirectory, DateTime.Now)
    {
    }

    /// <summary>
    /// Gets the source directory containing the files to be backed up.
    /// </summary>
    public string SourceDirectory => sourceDirectory;

    /// <summary>
    /// Gets the destination directory where the backup will be stored.
    /// </summary>
    public string DestinationDirectory => destinationDirectory;

    /// <summary>
    /// Gets the date and time when the backup is created.
    /// </summary>
    public DateTime BackupDateTime => backupDateTime;

    /// <summary>
    /// Gets or sets a value indicating whether to compress the backup files.
    /// </summary>
    public bool Compress { get; set; } = true;

    /// <summary>
    /// Gets the compression level to be used for the backup files.
    /// </summary>
    public CompressionLevel CompressionLevel =>
        Compress ? CompressionLevel.SmallestSize : CompressionLevel.NoCompression;

    /// <summary>
    /// Prepares the list of files to be backed up asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the total number of files prepared for backup.</returns>
    public async Task<int> PrepareFilesAsync(CancellationToken cancellationToken = default)
    {
        preparedFiles = await Task.Run(() => Directory.GetFiles(
            SourceDirectory,
            "*.*",
            SearchOption.AllDirectories
        ), cancellationToken);

        return preparedFiles.Count;
    }

    /// <summary>
    /// Executes the backup process asynchronously and creates a .tar.gz file containing the backed-up files.
    /// </summary>
    /// <param name="progress">An optional progress reporter to provide updates during the backup process.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the backup operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the full path of the created backup file.</returns>
    public async Task<string> ExecuteAsync(IProgress<(string, int)>? progress = null, CancellationToken cancellationToken = default)
    {
        string filename = $"rp-backup.{BackupDateTime:yyyyMMddHHmmss}.tar.gz";
        string fullpath = Path.Combine(DestinationDirectory, filename);

        await using var writer = OpenTarWriter(fullpath);
        IList<string> files = preparedFiles;

        for (int i = 0; i < files.Count; i++)
        {
            string entryFile = files[i];
            string entryName = Path.GetRelativePath(SourceDirectory, entryFile).Replace('\\', '/');

            progress?.Report((entryName, i + 1));
            await writer.WriteEntryAsync(entryFile, entryName, cancellationToken);
        }

        return fullpath;
    }

    /// <summary>
    /// Opens a <see cref="TarWriter"/> for writing a .tar.gz file.
    /// </summary>
    /// <param name="filename">The name of the file to create.</param>
    /// <returns>A <see cref="TarWriter"/> instance for writing to the specified file.</returns>
    private TarWriter OpenTarWriter(string filename)
    {
        var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        var gzipStream = new GZipStream(fileStream, CompressionLevel, leaveOpen: false);
        return new TarWriter(gzipStream, TarEntryFormat.Pax, leaveOpen: false);
    }
}
