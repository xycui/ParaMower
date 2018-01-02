using System;

namespace ParaMower.Supports
{
    public class CommandParamAttribute : Attribute
    {
        public string ParamAlias;
        public string Description;

        public CommandParamAttribute(string paramAlias)
        {
            ParamAlias = paramAlias;
        }

        public CommandParamAttribute(string paramAlias, string description)
        {
            ParamAlias = paramAlias;
            Description = description;
        }
    }
}
