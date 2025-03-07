#nullable enable

using System.Collections.Generic;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class CommandBuilder
    {
        private readonly IList<IStatement> _statements = new List<IStatement>();

        public CommandBuilder Append(IStatement statement)
        {
            _statements.Add(statement);
            return this;
        }

        public CommandBuilder Append(IStatement statement, int num)
        {
            for (int i = 0; i < num; i++)
            {
                _statements.Add(statement);
            }
            return this;
        }

        public CommandBuilder AppendFirst(IStatement statement)
        {
            _statements.Insert(0, statement);
            return this;
        }

        public CommandBuilder AppendFirst(IStatement statement, int num)
        {
            for (int i = 0; i < num; i++)
            {
                _statements.Insert(0, statement);
            }
            return this;
        }

        public CommandBuilder Remove(IStatement statement)
        {
            int index = _statements.IndexOf(statement);
            _statements.RemoveAt(index);
            return this;
        }

        public CommandBuilder Remove(int index)
        {
            _statements.RemoveAt(index);
            return this;
        }

        public CommandBuilder Remove(int startIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (_statements.Count < startIndex)
                {
                    break;
                }
                _statements.RemoveAt(startIndex);
            }
            return this;
        }

        public CommandBuilder Clear()
        {
            _statements.Clear();
            return this;
        }

        public IList<IStatement> Build()
        {
            return _statements;
        }
    }
}
