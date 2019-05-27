using System.Collections.Generic;
using System.Linq;
using BabelFish.AST;
using enquanto.Model;
using sly.lexer;
using sly.parser.generator;

namespace enquanto
{
    public class Parser
    {
        [Operation((int)EnquantoToken.LESSER, Affix.InFix, Associativity.Right, 50)]
        [Operation((int)EnquantoToken.GREATER, Affix.InFix, Associativity.Right, 50)]
        [Operation((int)EnquantoToken.EQUALS, Affix.InFix, Associativity.Right, 50)]
        [Operation((int)EnquantoToken.DIFFERENT, Affix.InFix, Associativity.Right, 50)]
        public INode<EnquantoType> BinaryComparisonExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.ADD;

            switch (operatorToken.TokenID)
            {
                case EnquantoToken.LESSER:
                    {
                        oper = BinaryOperator.LESSER;
                        break;
                    }
                case EnquantoToken.GREATER:
                    {
                        oper = BinaryOperator.GREATER;
                        break;
                    }
                case EnquantoToken.EQUALS:
                    {
                        oper = BinaryOperator.EQUALS;
                        break;
                    }
                case EnquantoToken.DIFFERENT:
                    {
                        oper = BinaryOperator.DIFFERENT;
                        break;
                    }
            }

            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Operation((int)EnquantoToken.CONCAT, Affix.InFix, Associativity.Right, 10)]
        public INode<EnquantoType> BinaryStringExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.CONCAT;
            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Production("statement :  LPAREN [d] statement RPAREN [d]")]
        public INode<EnquantoType> Block(INode<EnquantoType> statement)
        {
            return statement as IStatement<EnquantoType>;
        }

        [Production("statement : sequence")]
        public INode<EnquantoType> StatementSequence(INode<EnquantoType> sequence)
        {
            return sequence as IStatement<EnquantoType>;
        }

        [Production("sequence : statementPrim additionalStatements*")]
        public INode<EnquantoType> SequenceStatements(INode<EnquantoType> first, IEnumerable<INode<EnquantoType>> next)
        {
            var seq = new SequenceStatement(first as IStatement<EnquantoType>);
            seq?.AddRange(next.Cast<IStatement<EnquantoType>>().ToList());
            return seq;
        }

        [Production("additionalStatements : SEMICOLON [d] statementPrim")]
        public INode<EnquantoType> Additional(INode<EnquantoType> statement)
        {
            return statement as IStatement<EnquantoType>;
        }

        [Production("statementPrim: IF [d] Parser_expressions THEN [d] statement ELSE [d] statement")]
        public INode<EnquantoType> IfStmt(INode<EnquantoType> cond, INode<EnquantoType> thenStmt, INode<EnquantoType> elseStmt)
        {
            var stmt = new IfStatement(cond as IExpression<EnquantoType>, thenStmt as IStatement<EnquantoType>, elseStmt as IStatement<EnquantoType>);
            return stmt as IStatement<EnquantoType>;
        }

        [Production("statementPrim: WHILE [d] Parser_expressions DO [d] statement")]
        public INode<EnquantoType> WhileStmt(INode<EnquantoType> cond, INode<EnquantoType> blockStmt)
        {
            var stmt = new WhileStatement(cond as IExpression<EnquantoType>, blockStmt as IStatement<EnquantoType>);
            return stmt as IStatement<EnquantoType>;
        }

        [Production("statementPrim: IDENTIFIER ASSIGN [d] Parser_expressions")]
        public INode<EnquantoType> AssignStmt(Token<EnquantoType> variable, INode<EnquantoType> value)
        {
            var assign = new AssignStatement(variable.StringWithoutQuotes, value as IExpression<EnquantoType>);
            return assign as IStatement<EnquantoType>;
        }

        [Production("statementPrim: SKIP [d]")]
        public INode<EnquantoType> SkipStmt()
        {
            return new SkipStatement();
        }

        [Production("statementPrim: RETURN [d] Parser_expressions")]
        public INode<EnquantoType> ReturnStmt(INode<EnquantoType> expression)
        {
            return new ReturnStatement(expression as IExpression<EnquantoType>);
        }

        [Production("statementPrim: PRINT [d] Parser_expressions")]
        public INode<EnquantoType> PrintStmt(INode<EnquantoType> expression)
        {
            return new PrintStatement(expression as IExpression<EnquantoType>);
        }

        [Production("primary: INT")]
        public INode<EnquantoType> PrimaryInt(Token<EnquantoType> intToken)
        {
            return new IntegerConstant(intToken.IntValue);
        }

        [Production("primary: TRUE")]
        [Production("primary: FALSE")]
        public INode<EnquantoType> PrimaryBool(Token<EnquantoType> boolToken)
        {
            return new BoolConstant(bool.Parse(boolToken.StringWithoutQuotes));
        }

        [Production("primary: STRING")]
        public INode<EnquantoType> PrimaryString(Token<EnquantoType> stringToken)
        {
            return new StringConstant(stringToken.StringWithoutQuotes);
        }

        [Production("primary: IDENTIFIER")]
        public INode<EnquantoType> PrimaryId(Token<EnquantoType> varToken)
        {
            return new Variable(varToken.StringWithoutQuotes);
        }

        [Operand]
        [Production("operand: primary")]
        public INode<EnquantoType> Operand(INode<EnquantoType> prim)
        {
            return prim;
        }

        [Operation((int)EnquantoToken.PLUS, Affix.InFix, Associativity.Right, 10)]
        [Operation((int)EnquantoToken.MINUS, Affix.InFix, Associativity.Right, 10)]
        public INode<EnquantoType> BinaryTermNumericExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.ADD;

            switch (operatorToken.TokenID)
            {
                case EnquantoToken.PLUS:
                    {
                        oper = BinaryOperator.ADD;
                        break;
                    }
                case EnquantoToken.MINUS:
                    {
                        oper = BinaryOperator.SUB;
                        break;
                    }
            }

            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Operation((int)EnquantoToken.TIMES, Affix.InFix, Associativity.Right, 50)]
        [Operation((int)EnquantoToken.DIVIDE, Affix.InFix, Associativity.Right, 50)]
        public INode<EnquantoType> BinaryFactorNumericExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.MULTIPLY;

            switch (operatorToken.TokenID)
            {
                case EnquantoToken.TIMES:
                    {
                        oper = BinaryOperator.MULTIPLY;
                        break;
                    }
                case EnquantoToken.DIVIDE:
                    {
                        oper = BinaryOperator.DIVIDE;
                        break;
                    }
            }

            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Operation((int)EnquantoToken.MINUS, Affix.PreFix, Associativity.Right, 100)]
        public INode<EnquantoType> UnaryNumericExpression(Token<EnquantoType> operation, INode<EnquantoType> value)
        {
            return new Neg(value as IExpression<EnquantoType>);
        }

        [Operation((int)EnquantoToken.OR, Affix.InFix, Associativity.Right, 10)]
        public INode<EnquantoType> BinaryOrExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.OR;

            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Operation((int)EnquantoToken.AND, Affix.InFix, Associativity.Right, 50)]
        public INode<EnquantoType> BinaryAndExpression(INode<EnquantoType> left, Token<EnquantoToken> operatorToken, INode<EnquantoType> right)
        {
            var oper = BinaryOperator.AND;

            var operation = new BinaryOperation(left as IExpression<EnquantoType>, oper, right as IExpression<EnquantoType>);
            return operation;
        }

        [Operation((int)EnquantoToken.NOT, Affix.PreFix, Associativity.Right, 100)]
        public INode<EnquantoType> BinaryOrExpression(Token<EnquantoToken> operatorToken, INode<EnquantoType> value)
        {
            return new Not(value as IExpression<EnquantoType>);
        }
    }
}
