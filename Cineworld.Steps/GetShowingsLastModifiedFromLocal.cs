using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class GetShowingsLastModifiedFromLocal : StepBody
	{
		private readonly Services.IFileSystemService _fileSystemService;

		public GetShowingsLastModifiedFromLocal(
			Services.IFileSystemService fileSystemService)
		{
			_fileSystemService = fileSystemService
				?? throw new ArgumentOutOfRangeException(nameof(fileSystemService));
		}

		public DateTime? LastModified { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			LastModified = _fileSystemService.GetShowingsLastModified();

			return ExecutionResult.Next();
		}
	}
}
