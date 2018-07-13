﻿using Calamari.Integration.FileSystem;
using Calamari.Integration.Processes;
using Octopus.CoreUtilities.Extensions;

namespace Calamari.Util
{
    public class TemplateService
    {
        private readonly ICalamariFileSystem fileSystem;
        private readonly ITemplateResolver resolver;
        private readonly ITemplateReplacement replacement;

        public TemplateService(ICalamariFileSystem fileSystem, ITemplateResolver resolver, ITemplateReplacement replacement)
        {
            this.fileSystem = fileSystem;
            this.resolver = resolver;
            this.replacement = replacement;
        }

        public string GetTemplateContent(string relativePath, bool inPackage, CalamariVariableDictionary variables)
        {
            return resolver.Resolve(relativePath, inPackage, variables)
                .Map(x => x.Value)
                .Map(fileSystem.ReadFile);
        }

        public string GetSubstitutedTemplateContent(string relativePath, bool inPackage, CalamariVariableDictionary variables)
        {
            return replacement.ResolveAndSubstituteFile(
                () => resolver.Resolve(relativePath, inPackage, variables).Value,
                fileSystem.ReadFile,
                variables);
        }
    }
}