﻿using Calamari.Integration.ServiceMessages;
using Octostache;

namespace Calamari.Integration.Processes
{
    public class CalamariCommandOutput : ICommandOutput
    {
        private readonly VariableDictionary variables;

        public CalamariCommandOutput(VariableDictionary variables)
        {
            this.variables = variables;
        }

        public void WriteInfo(string line)
        {
            var serviceMessageParser = new ServiceMessageParser(ProcessServiceMessage);
            serviceMessageParser.Parse(line);
        }

        public void WriteError(string line)
        {
        }

        private void ProcessServiceMessage(ServiceMessage message)
        {
            switch (message.Name)
            {
                case ServiceMessageNames.SetVariable.Name:
                    var variableName = message.GetValue(ServiceMessageNames.SetVariable.NameAttribute);
                    var variableValue = message.GetValue(ServiceMessageNames.SetVariable.ValueAttribute);

                    if (!string.IsNullOrWhiteSpace(variableName))
                        variables.SetOutputVariable(variableName, variableValue);
                    break;
            }
        }
    }
}