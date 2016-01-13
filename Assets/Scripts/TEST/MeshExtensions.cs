using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshExtensions { // Extension classes need to be static

	public static void SetVertexColor( this Mesh mesh, Color color ) {
		var VertColors = new Color[mesh.vertexCount];  // Look into differences between var and int
		for( var i = 0; i < mesh.vertexCount; i++ ) {
			VertColors[i] = color;
		}
		mesh.colors = VertColors;
		mesh.RecalculateNormals ();
	}

	public static void SetVertexColor( this Mesh mesh, Color topColor, Color bottomColor, float verticalOffset ) {
		var minY = float.MaxValue;
		var maxY = float.MinValue;

		for( var i = 0; i < mesh.vertices.Length; i++ ) {
			if( mesh.vertices[i].y > maxY ) {
				maxY = mesh.vertices[i].y;
			}
			else if( mesh.vertices[i].y < minY ) {
				minY = mesh.vertices[i].y;
			}
		}

		var meshHeight = maxY - minY;

		var vertColors = new List<Color>();
		for( var i = 0; i < mesh.vertexCount; i++ ) {
			var t = (minY + mesh.vertices[i].y ) / meshHeight;
			t = Mathf.Abs (t);
			vertColors.Add ( Color.Lerp (topColor, bottomColor, t - verticalOffset ));
		}

		mesh.colors = vertColors.ToArray ();
		mesh.RecalculateNormals ();
	}
}
