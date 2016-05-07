using Artemis.Interface;

public class PositionComponent : IComponent, IReset {

	int x = 0;
	int y = 100;
	int z = 0;

	public void Initialize (params object[] args)
	{
		X = 0;
		y = 0;
		Z = 0;
	}
		
	public int X { get { return x; } set { x = value; } }
	public int Y { get { return y; } set { y = value; } }
	public int Z { get { return z; } set { z = value; } }
}
