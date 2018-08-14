using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class Save : StepBodyAsync
	{
		private readonly Services.IFileSystemService _fileSystemService;

		public Save(
			Services.IFileSystemService fileSystemService)
		{
			_fileSystemService = fileSystemService;
		}

		public Models.cinemasType Cinemas { get; set; }
		public DateTime LastModified { get; set; }

		public async override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
		{
			await _fileSystemService.SaveShowingsAsync(Cinemas, LastModified)
				.ConfigureAwait(false);

			return ExecutionResult.Next();
		}
	}
}
