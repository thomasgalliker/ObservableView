using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;
using ObservableView.Searching.Operators;

namespace ObservableView.Extensions
{
    internal static class OperationExtensions
    {
        internal static IEnumerable<object> Flatten(this IOperation operation)
        {
            return Recurse(operation);
        }

        private static IEnumerable<object> Recurse(object obj)
        {
            if (obj is BinaryOperation binaryOperation)
            {
                yield return binaryOperation.LeftOperand;

                if (binaryOperation.Operator is IOperator @operator)
                {
                    yield return @operator;
                }

                yield return binaryOperation.RightOperand;
            }

            if (obj is GroupOperation groupOperation)
            {
                foreach (var groupObject in RecurseGroupOperation(groupOperation))
                {
                    yield return groupObject;
                }
            }
        }

        private static IEnumerable<object> RecurseGroupOperation(GroupOperation groupOperation)
        {
            foreach (var binObject in Recurse(groupOperation.LeftOperation))
            {
                yield return binObject;
            }

            if (groupOperation.Operator is IOperator @operator)
            {
                yield return @operator;
            }

            foreach (var binObject in Recurse(groupOperation.RightOperation))
            {
                yield return binObject;
            }
        }
    }
}