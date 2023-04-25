using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
	public enum Type
	{
		Pawn,
		Knight,
		Bishop,
		Rook,
		Queen,
		King
	}

	public enum Color
	{
		White,
		Black
	}

	public enum Row
	{
		_a,
		_b,
		_c,
		_d,
		_e,
		_f,
		_g,
		_h
	}

	public enum Column
	{
		_1,
		_2,
		_3,
		_4,
		_5,
		_6,
		_7,
		_8
	}
}
