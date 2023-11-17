//ConsoleApp1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public enum Token_Class
{
	NUMBER, IDENTIFIER,

	//Operators
	PLUS, MINUS, MULT, DIV, ASSIGN, EQUAL, 
	LESSTHAN, SEMICOLON, OPENBRACKET, CLOSEDBRACKET,

	//Reserved words
	 READ,WRITE, REPEAT, UNTIL, IF, THEN, END
}
namespace ConsoleApp1
{


	public class Token
	{
		public string lex = "";
		public Token_Class token_type;

		public override string ToString()
		{
			return $"{lex}\t,{token_type}";
		}
	}

	public class Scanner
	{
		public List<Token> Tokens = new();

        readonly Dictionary<string, Token_Class> ReservedWords = new ();
		readonly Dictionary<string, Token_Class> Operators = new ();

		public Scanner()
		{

			ReservedWords.Add("read", Token_Class.READ);
			ReservedWords.Add("write", Token_Class.WRITE);
			ReservedWords.Add("repeat", Token_Class.REPEAT);
			ReservedWords.Add("until", Token_Class.UNTIL);
			ReservedWords.Add("if", Token_Class.IF);
			ReservedWords.Add("then", Token_Class.THEN);
			ReservedWords.Add("end", Token_Class.END);
			//Operators.Add(".", Token_Class.T_Dot);
			Operators.Add(";", Token_Class.SEMICOLON);
			Operators.Add("(", Token_Class.CLOSEDBRACKET);
			Operators.Add(")", Token_Class.OPENBRACKET);
			Operators.Add("<", Token_Class.LESSTHAN);
			Operators.Add("-", Token_Class.MINUS);
			Operators.Add("+", Token_Class.PLUS);
			Operators.Add("*", Token_Class.MULT);
			Operators.Add("/", Token_Class.DIV);
			Operators.Add(":=", Token_Class.ASSIGN);
			Operators.Add("=", Token_Class.EQUAL);
		}

		public void StartScanning(string SourceCode)
		{
			for (int i = 0; i < SourceCode.Length; i++)
			{
				int j = i;
				char CurrentChar = SourceCode[i];
				string CurrentLexeme = CurrentChar.ToString();

				if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n') //Whitespace
					continue;

				if (Errors.Error_List.Count > 0)
					break;

				if (SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] >= 'A' && SourceCode[j] <= 'z') //Identifier lexeme
				{
					j++;
					while (j < SourceCode.Length)
					{
						if (SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] >= 'A' && SourceCode[j] <= 'z')
						{
							CurrentLexeme += SourceCode[j].ToString();
						}
						else break; 
						j++;
					}
					FindTokenClass(CurrentLexeme);
					i = j - 1;
					continue;
				}
				else if (CurrentChar == '{') //Comment lexeme to disregard
				{
					bool closed = false;
					j++;
					if (j < SourceCode.Length)
					{
						CurrentLexeme += SourceCode[j].ToString();
						j++;
						try
						{
							while (j < SourceCode.Length)
							{
								CurrentLexeme += SourceCode[j].ToString();
								j++;
								if (SourceCode[j] == '}')
								{
									CurrentLexeme += SourceCode[j].ToString();
									closed = true;
									i = j;
									break;
								}
							}
						}
						catch (Exception)
						{
							Errors.Error_List.Add("Comment not closed");
							//i = j;
							break;
						}
						if (!closed)
						{
							Errors.Error_List.Add("Comment not closed");
							//i = j;
							break;
						}

					}
					//FindTokenClass(CurrentLexeme);
				}
				else if(CurrentChar == '}')
				{
                    Errors.Error_List.Add("Comment not opened");
					break;
                }
                //To handle assignment operator, because it is the only OP with two characters
                else if (CurrentChar == ':')
				{
					j++;
					if (j < SourceCode.Length && SourceCode[j] == '=')
					{
						CurrentLexeme += SourceCode[j].ToString();
					}

				}
				else
				{
					if (Operators.ContainsKey(CurrentLexeme))
					{
						FindTokenClass(CurrentLexeme);
						continue;
					}
					else
					{
						j++;
						while (j < SourceCode.Length)
						{
							if (!(SourceCode[j] == ' ' || SourceCode[j] == '\r' || SourceCode[j] == '\n'))
							{
								CurrentLexeme += SourceCode[j];
								j++;
							}
							else { break; }
						}


					}

				}
				FindTokenClass(CurrentLexeme.Trim());
				i = j;
			}
		}
		void FindTokenClass(string Lex)
		{
			Token_Class TC;
			Token Tok = new();
			Tok.lex = Lex;

			//Is it a reserved word?
			if (ReservedWords.ContainsKey(Lex))
			{
				Tok.token_type = ReservedWords[Lex];
				Tokens.Add(Tok);
			}
			//Is it an identifier?
			else if (isIdentifier(Lex))
			{
				Tok.token_type = Token_Class.IDENTIFIER;
				Tokens.Add(Tok);
			}
			//Is it a Constant?
			else if (isNumber(Lex))
			{
				Tok.token_type = Token_Class.NUMBER;
				Tokens.Add(Tok);
			}
			//Is it an operator?
			else if (Operators.ContainsKey(Lex))
			{
				Tok.token_type = Operators[Lex];
				Tokens.Add(Tok);
			}
			else if (Lex[0] == '{' && Lex[Lex.Length - 1] == '}')
			{
				//Do Noting
			}
			//Is it an undefined?
			else
			{
				Errors.Error_List.Add("Unrecognized token: " + Lex);
			}
		}



		bool isIdentifier(string lex)
		{
			// Check if the lex is an identifier or not.

			if (((lex[0] >= 'A' && lex[0] <= 'Z') || (lex[0] >= 'a' && lex[0] <= 'z') || lex[0] == '_'))
			{
				for (int i = 1; i < lex.Length; i++)
				{
					if ((lex[i] >= 'A' && lex[i] <= 'Z') || (lex[i] >= 'a' && lex[i] <= 'z') || lex[i] == '_') continue;
					else return false;
				}
				return true;
			}
			else
			{
				return false;
			}


		}
		bool isNumber(string lex)
		{
			if (lex.Length > 0 && (lex[0] >= '0' && lex[0] <= '9')) //starts with a digit
			{
				for (int i = 1; i < lex.Length; i++)
				{
					if ((lex[i] >= '0' && lex[i] <= '9'))
					{
						continue;
					}
					else
					{
						return false;
					}
				}
				return true;
			}
			else
			{
				return false;
			}

		}
	}
}