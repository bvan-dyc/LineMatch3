using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{

	public int x { get; private set; }

	public int y { get; private set; }

	public int Z { get { return -x - y; } }

	public HexCoordinates(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static HexCoordinates FromOffsetCoordinates(int x, int y)
	{
		return new HexCoordinates(x, y - x / 2);
	}

	public static bool operator ==(HexCoordinates a, HexCoordinates b)
	{
		return (a.x == b.x && a.y == b.y && a.Z == b.Z);
	}

	public static bool operator !=(HexCoordinates a, HexCoordinates b)
	{
		return !(a == b);
	}

	public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
	{
		return new HexCoordinates(a.x + b.x, a.y + b.y);
	}

	public static HexCoordinates operator -(HexCoordinates c1, HexCoordinates c2)
	{
		return new HexCoordinates(c1.x - c2.x, c1.y - c2.y);
	}

	public override bool Equals(object obj)
	{
		if (obj is HexCoordinates)
		{
			HexCoordinates c = (HexCoordinates)obj;
			return x == c.x && y == c.y && Z == c.Z;
		}
		return false;
	}

	public bool Equals(HexCoordinates c)
	{
		return x == c.x && y == c.y && Z == c.Z;
	}

	public override int GetHashCode()
	{
		return x ^ y;
	}
}
