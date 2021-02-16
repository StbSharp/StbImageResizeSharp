using StbImageSharp;
using StbImageWriteSharp;
using System;
using System.IO;

namespace StbImageResizeSharp.Converter
{
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		private enum OutputType
		{
			Jpg,
			Png,
			Tga,
			Bmp,
			Hdr
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Usage: StbImageResizeSharp.Converter.exe <input_image_file> <output_image_file> [new_width] [new_height]");
				Console.WriteLine("Example: StbImageResizeSharp.Converter.exe image.jpg image.png 400 400");
				return;
			}

			try
			{
				OutputType outputType = OutputType.Png;

				var ext = Path.GetExtension(args[1]);
				if (string.IsNullOrEmpty(ext))
				{
					throw new Exception("Output file lacks extension. Hence it is not possible to determine output file type");
				}

				if (ext.StartsWith("."))
				{
					ext = ext.Substring(1);
				}

				ext = ext.ToLower();
				switch (ext)
				{
					case "jpg":
					case "jpeg":
						outputType = OutputType.Jpg;
						break;
					case "png":
						outputType = OutputType.Png;
						break;
					case "tga":
						outputType = OutputType.Tga;
						break;
					case "bmp":
						outputType = OutputType.Bmp;
						break;
					case "hdr":
						outputType = OutputType.Hdr;
						break;
					default:
						throw new Exception("Output format '" + ext + "' is not supported.");
				}

				// Load image
				ImageResult image;

				using (var stream = File.OpenRead(args[0]))
				{
					image = ImageResult.FromStream(stream);
				}

				// Parse new size
				var newWidth = image.Width;
				var newHeight = image.Height;

				if (args.Length >= 3)
				{
					newWidth = int.Parse(args[2]);
				}

				if (args.Length >= 4)
				{
					newHeight = int.Parse(args[3]);
				}

				var newData = image.Data;
				var channels = (int)image.Comp;

				// Resize if needed
				if (newWidth != image.Width && newHeight != image.Height)
				{
					newData = new byte[newWidth * newHeight * channels];

					StbImageResize.stbir_resize_uint8(image.Data, image.Width, image.Height, image.Width * channels,
						newData, newWidth, newHeight, newWidth * channels, channels);
				}

				// Save
				using (var stream = File.Create(args[1]))
				{
					var imageWriter = new ImageWriter();
					switch (outputType)
					{
						case OutputType.Jpg:
							imageWriter.WriteJpg(newData, newWidth, newHeight, (StbImageWriteSharp.ColorComponents)channels, stream, 90);
							break;
						case OutputType.Png:
							imageWriter.WritePng(newData, newWidth, newHeight, (StbImageWriteSharp.ColorComponents)channels, stream);
							break;
						case OutputType.Tga:
							imageWriter.WriteTga(newData, newWidth, newHeight, (StbImageWriteSharp.ColorComponents)channels, stream);
							break;
						case OutputType.Bmp:
							imageWriter.WriteBmp(newData, newWidth, newHeight, (StbImageWriteSharp.ColorComponents)channels, stream);
							break;
						case OutputType.Hdr:
							imageWriter.WriteHdr(newData, newWidth, newHeight, (StbImageWriteSharp.ColorComponents)channels, stream);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}