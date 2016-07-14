using UnityEngine;
using System.Collections;

public class Int3 {

    public int x = 0;
    public int y = 0;
    public int z = 0;

	public Int3() {

    }
    public Int3(int xi, int yi, int zi) {
        x = xi;
        y = yi;
        z = zi;
    }

    public override string ToString() {
        return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
    }
}
