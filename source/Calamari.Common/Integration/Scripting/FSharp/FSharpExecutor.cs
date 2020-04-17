﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Calamari.Integration.Processes;

namespace Calamari.Common.Integration.Scripting.FSharp
{
    public class FSharpExecutor : ScriptExecutor
    {
        protected override IEnumerable<ScriptExecution> PrepareExecution(Script script, IVariables variables,
            Dictionary<string, string> environmentVars = null)
        {
            var workingDirectory = Path.GetDirectoryName(script.File);
            var executable = FSharpBootstrapper.FindExecutable();
            var configurationFile = FSharpBootstrapper.PrepareConfigurationFile(workingDirectory, variables);
            var (bootstrapFile, otherTemporaryFiles) = FSharpBootstrapper.PrepareBootstrapFile(script.File, configurationFile, workingDirectory, variables);
            var arguments = FSharpBootstrapper.FormatCommandArguments(bootstrapFile, script.Parameters);

            yield return new ScriptExecution(
                new CommandLineInvocation(executable, arguments)
                {
                    WorkingDirectory = workingDirectory,
                    EnvironmentVars = environmentVars
                },
                otherTemporaryFiles.Concat(new[] {bootstrapFile, configurationFile})
            );
        }
    }
}