// Copyright © 2023 TradingLens. All Rights Reserved.

using Stigs.Utils.Common;
using Stigs.Utils.Common.DependencyInjection;

namespace StigsTool.Core.Commands;

public class AddSolutionConfigFilesCommand : CommandBase<AddSolutionConfigFilesCommand> {

  [Singleton]
  internal sealed class Handler : CommandHandlerBase<AddSolutionConfigFilesCommand> {
    protected override Task OnExecute(AddSolutionConfigFilesCommand command) {
      throw new NotImplementedException();
    }
  }
}

