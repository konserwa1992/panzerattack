﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PanzerRush;

namespace PanzerRush.Map
{
	class Layer
	{
		public int LayerIndex { get; set; }
		public string Name { get; set; }
		public int[,] MapGridsData { get; set; }
		public int SheetID { get; set; }
	
		private VertexBuffer LayerVertexBuffer { set; get; }
		private GridsSheetContainer GridSheet { set; get; }

		public void InitMesh(GraphicsDevice device, List<GridsSheetContainer> gridTexSheets, ContentManager content)
		{
			List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
			GridSheet = (from gridSheet in gridTexSheets where gridSheet.ID == SheetID select gridSheet).FirstOrDefault();

			if (GridSheet != null)
			{
				for (int x = 0; x < MapGridsData.GetLength(0); x++)
				{
					for (int y = 0; y <MapGridsData.Length/MapGridsData.GetLength(0); y++)
					{
						if (MapGridsData[x, y] != 0)
						{
							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3(x * 64, LayerIndex, y * 64),
								new Vector3(0, 1, 0),
								new Vector2(((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X / (float)GridSheet.TextureSizeX), ((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y / (float)GridSheet.TextureSizeY))));

							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3((x + 1) * 64, LayerIndex, y * 64),
								new Vector3(0, 1, 0),
								new Vector2((((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X+(float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeX) / (float)GridSheet.TextureSizeX), ((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y / (float)GridSheet.TextureSizeY))));

							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3((x + 1) * 64, LayerIndex, (y + 1) * 64),
								new Vector3(0, 1, 0),
								new Vector2((((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X + (float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeX) / (float)GridSheet.TextureSizeX), (((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y+(float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeY) / (float)GridSheet.TextureSizeY))));

							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3(x * 64, LayerIndex, y * 64),
								new Vector3(0, 1, 0),
								new Vector2(((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X / (float)GridSheet.TextureSizeX), ((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y / (float)GridSheet.TextureSizeY))));

							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3((x + 1) * 64, LayerIndex, (y + 1) * 64),
								new Vector3(0, 1, 0),
								new Vector2((((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X+(float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeX) / (float)GridSheet.TextureSizeX), (((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y+(float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeY) / (float)GridSheet.TextureSizeY))));

							verticesList.Add(new VertexPositionNormalTexture(
								new Vector3(x * 64, LayerIndex, (y + 1) * 64),
								new Vector3(0, 1, 0),
								new Vector2(((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).X / (float)GridSheet.TextureSizeX), (((float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).Y+(float)GridSheet.GridsDataList.FirstOrDefault(g => g.ID == MapGridsData[x, y]).SizeY) / (float)GridSheet.TextureSizeY))));
						}
					}
				}
			}
			else
			{
				using (StreamWriter LogWriter = new StreamWriter("Log.txt", true))
				{
					LogWriter.WriteLine($"SheetID is invalid:{SheetID} LayerName:{Name}");
				}
			}

			LayerVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);
			LayerVertexBuffer.SetData<VertexPositionNormalTexture>(verticesList.ToArray());
		}

		public void Draw(GraphicsDevice device, Camera camera)
		{

			BasicEffect basicEffect = new BasicEffect(device);
			basicEffect.World = Matrix.CreateTranslation(Vector3.Zero);
			basicEffect.View = camera.View;
			basicEffect.Projection = camera.Projection;
			basicEffect.TextureEnabled = true;	
			basicEffect.Texture = GridSheet.Texture;
			basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
			basicEffect.GraphicsDevice.BlendFactor = new Color(new Vector3(54/255,29/255,29/255));

			device.SetVertexBuffer(LayerVertexBuffer);

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.DrawPrimitives(PrimitiveType.TriangleList, 0, LayerVertexBuffer.VertexCount / 3);
			}
		}
	}

}
