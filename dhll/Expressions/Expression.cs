using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static dhll.v1.templateParser;

namespace dhll.Expressions;


// ==============================================================================================================================
public enum EOperator
{
  Invalid,
  Add,
  Subtract,
  Multiply,
  Divide,
}

// ==============================================================================================================================
public enum EPrimaryType
{
  Invalid = 0,
  Number,
  String,
  Identifier,
  Call,                 // A call to a function.
  Parenthesis           // An expression wrapped in parenthesis.
}

// ==============================================================================================================================
/// <summary>
/// This is how we are representing expressions / expression trees in dhll.
/// </summary>
public class Expression
{

}


// ==============================================================================================================================
/// <summary>
/// Binary expressions, like add, subtract, etc.
/// </summary>
public class BinaryExpression : Expression
{
  // --------------------------------------------------------------------------------------------------------------------------
  public BinaryExpression(Expression left_, Expression right_, EOperator opType_)
  {
    Left = left_;
    Right = right_;
    OperatorType = opType_;
  }

  public EOperator OperatorType { get; set; }
  public Expression Left { get; set; } = default!;
  public Expression Right { get; set; } = default!;
}


// ==============================================================================================================================
/// <summary>
/// Primary expressions are literal values, numbers, identifiers, etc.
/// </summary>
public class PrimaryExpression : Expression
{
  public EPrimaryType Type { get; set; }
  public string Content { get; set; }

  // --------------------------------------------------------------------------------------------------------------------------
  public PrimaryExpression(VARIABLEContext input)
  {
    Type = EPrimaryType.Identifier;
    Content = input.GetText();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public PrimaryExpression(MAGIC_STRINGContext input)
  {
    Type = EPrimaryType.String;
    Content = input.GetText(); 
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public PrimaryExpression(MAGIC_NUMBERContext input)
  {
    Type = EPrimaryType.String;
    Content = input.GetText();
  }

}

