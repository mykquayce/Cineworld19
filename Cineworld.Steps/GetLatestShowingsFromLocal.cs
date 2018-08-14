using Cineworld.Models;
using Cineworld.Services;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class GetLatestShowingsFromLocal : StepBody
	{
		private readonly IFileSystemService _fileSystemService;

		public GetLatestShowingsFromLocal(
			IFileSystemService fileSystemService)
		{
			_fileSystemService = fileSystemService
				?? throw new ArgumentNullException(nameof(fileSystemService));
		}

		public cinemasType Cinemas { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			Cinemas = _fileSystemService.LoadLastestShowings();

			return ExecutionResult.Next();
		}
	}
}
