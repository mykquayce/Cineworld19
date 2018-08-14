using Cineworld.Models;
using Cineworld.Services;
using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class SaveShowingsToLocal : StepBodyAsync
	{
		private readonly IFileSystemService _fileSystemService;

		public SaveShowingsToLocal(
			IFileSystemService fileSystemService)
		{
			_fileSystemService = fileSystemService;
		}

		public cinemasType Cinemas { get; set; }
		public DateTime LastModified { get; set; }

		public async override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
		{
			await _fileSystemService.SaveShowingsAsync(Cinemas, LastModified).ConfigureAwait(false);

			return ExecutionResult.Next();
		}
	}
}
