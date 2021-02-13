using System.Runtime.InteropServices;

namespace StbImageResizeSharp
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	static unsafe partial class StbImageResize
	{
		public delegate float stbir__kernel_fn(float x, float scale);

		public delegate float stbir__support_fn(float scale);

		public enum stbir_colorspace
		{
			STBIR_COLORSPACE_LINEAR,
			STBIR_COLORSPACE_SRGB,
			STBIR_MAX_COLORSPACES
		}

		public enum stbir_datatype
		{
			STBIR_TYPE_UINT8,
			STBIR_TYPE_UINT16,
			STBIR_TYPE_UINT32,
			STBIR_TYPE_FLOAT,
			STBIR_MAX_TYPES
		}

		public enum stbir_edge
		{
			STBIR_EDGE_CLAMP = 1,
			STBIR_EDGE_REFLECT = 2,
			STBIR_EDGE_WRAP = 3,
			STBIR_EDGE_ZERO = 4
		}

		public enum stbir_filter
		{
			STBIR_FILTER_DEFAULT = 0,
			STBIR_FILTER_BOX = 1,
			STBIR_FILTER_TRIANGLE = 2,
			STBIR_FILTER_CUBICBSPLINE = 3,
			STBIR_FILTER_CATMULLROM = 4,
			STBIR_FILTER_MITCHELL = 5
		}

		public enum stbir__resize_flag
		{
			None,
			AlphaPremultiplied = 1 << 0,
			AlphaUsesColorspace = 1 << 1
		}

		public class stbir__filter_info
		{
			public stbir__kernel_fn kernel;
			public stbir__support_fn support;

			public stbir__filter_info(stbir__kernel_fn k, stbir__support_fn s)
			{
				kernel = k;
				support = s;
			}
		}

		public class stbir__info
		{
			public int alpha_channel;
			public int channels;
			public int colorspace;
			public float* decode_buffer;
			public int decode_buffer_pixels;
			public int decode_buffer_size;
			public int edge_horizontal;
			public int edge_vertical;
			public float* encode_buffer;
			public int encode_buffer_size;
			public uint flags;
			public float* horizontal_buffer;
			public int horizontal_buffer_size;
			public int horizontal_coefficient_width;
			public float* horizontal_coefficients;
			public int horizontal_coefficients_size;
			public stbir__contributors* horizontal_contributors;
			public int horizontal_contributors_size;
			public int horizontal_filter;
			public int horizontal_filter_pixel_margin;
			public int horizontal_filter_pixel_width;
			public int horizontal_num_contributors;
			public float horizontal_scale;
			public float horizontal_shift;
			public void* input_data;
			public int input_h;
			public int input_stride_bytes;
			public int input_w;
			public void* output_data;
			public int output_h;
			public int output_stride_bytes;
			public int output_w;
			public float* ring_buffer;
			public int ring_buffer_begin_index;
			public int ring_buffer_first_scanline;
			public int ring_buffer_last_scanline;
			public int ring_buffer_length_bytes;
			public int ring_buffer_num_entries;
			public int ring_buffer_size;
			public float s0;
			public float s1;
			public float t0;
			public float t1;
			public int type;
			public int vertical_coefficient_width;
			public float* vertical_coefficients;
			public int vertical_coefficients_size;
			public stbir__contributors* vertical_contributors;
			public int vertical_contributors_size;
			public int vertical_filter;
			public int vertical_filter_pixel_margin;
			public int vertical_filter_pixel_width;
			public int vertical_num_contributors;
			public float vertical_scale;
			public float vertical_shift;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct stbir__FP32
		{
			[FieldOffset(0)] public uint u;
			[FieldOffset(0)] public float f;
		}

		public static stbir__filter_info[] stbir__filter_info_table =
		{
			new stbir__filter_info(null, stbir__support_zero),
			new stbir__filter_info(stbir__filter_trapezoid, stbir__support_trapezoid),
			new stbir__filter_info(stbir__filter_triangle, stbir__support_one),
			new stbir__filter_info(stbir__filter_cubic, stbir__support_two),
			new stbir__filter_info(stbir__filter_catmullrom, stbir__support_two),
			new stbir__filter_info(stbir__filter_mitchell, stbir__support_two)
		};

		public static byte stbir__linear_to_srgb_uchar(float _in_)
		{
			var almostone = new stbir__FP32 { u = 0x3f7fffff };
			var minval = new stbir__FP32 { u = (127 - 13) << 23 };
			uint tab;
			uint bias;
			uint scale;
			uint t;
			var f = new stbir__FP32();
			if (!(_in_ > minval.f))
				_in_ = minval.f;
			if (_in_ > almostone.f)
				_in_ = almostone.f;
			f.f = _in_;
			tab = fp32_to_srgb8_tab4[(f.u - minval.u) >> 20];
			bias = (tab >> 16) << 9;
			scale = tab & 0xffff;
			t = (f.u >> 12) & 0xff;
			return (byte)((bias + scale * t) >> 16);
		}

		public static int stbir_resize_uint8(byte[] input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			byte[] output_pixels, int output_w, int output_h, int output_stride_in_bytes, int num_channels)
		{
			fixed (byte* inputPtr = input_pixels)
			fixed (byte* outputPtr = output_pixels)
			{
				return stbir_resize_uint8(inputPtr, input_w, input_h, input_stride_in_bytes, outputPtr, output_w, output_h, output_stride_in_bytes, num_channels);
			}
		}

		public static int stbir_resize_float(float[] input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			float[] output_pixels, int output_w, int output_h, int output_stride_in_bytes, int num_channels)
		{
			fixed (float* inputPtr = input_pixels)
			fixed (float* outputPtr = output_pixels)
			{
				return stbir_resize_float(inputPtr, input_w, input_h, input_stride_in_bytes, outputPtr, output_w, output_h, output_stride_in_bytes, num_channels);
			}
		}

		public static int stbir_resize_uint8_srgb(byte[] input_pixels, int input_w, int input_h,
			int input_stride_in_bytes, byte[] output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			int num_channels, int alpha_channel, stbir__resize_flag flags)
		{
			fixed (byte* inputPtr = input_pixels)
			fixed (byte* outputPtr = output_pixels)
			{
				return stbir_resize_uint8_srgb(inputPtr, input_w, input_h, input_stride_in_bytes, outputPtr, output_w, output_h, output_stride_in_bytes,
					num_channels, alpha_channel, flags);
			}
		}

		public static int stbir_resize_uint8_srgb_edgemode(byte[] input_pixels, int input_w, int input_h,
			int input_stride_in_bytes, byte[] output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			int num_channels, int alpha_channel, stbir__resize_flag flags, stbir_edge edge_wrap_mode)
		{
			fixed (byte* inputPtr = input_pixels)
			fixed (byte* outputPtr = output_pixels)
			{
				return stbir_resize_uint8_srgb_edgemode(inputPtr, input_w, input_h, input_stride_in_bytes,
					outputPtr, output_w, output_h, output_stride_in_bytes,
					num_channels, alpha_channel, (int)flags, (int)edge_wrap_mode);
			}
		}

		public static int stbir_resize_uint8_generic(byte[] input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			byte[] output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			int num_channels, int alpha_channel, stbir__resize_flag flags, stbir_edge edge_wrap_mode, stbir_filter filter, stbir_colorspace space)
		{
			fixed (byte* inputPtr = input_pixels)
			fixed (byte* outputPtr = output_pixels)
			{
				return stbir_resize_uint8_generic(inputPtr, input_w, input_h, input_stride_in_bytes,
					outputPtr, output_w, output_h, output_stride_in_bytes,
					num_channels, alpha_channel, (int)flags, (int)edge_wrap_mode, (int)filter, (int)space);
			}
		}

		public static int stbir_resize_uint16_generic(ushort[] input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			ushort[] output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			int num_channels, int alpha_channel, stbir__resize_flag flags, stbir_edge edge_wrap_mode, stbir_filter filter, stbir_colorspace space)
		{
			fixed (ushort* inputPtr = input_pixels)
			fixed (ushort* outputPtr = output_pixels)
			{
				return stbir_resize_uint16_generic(inputPtr, input_w, input_h, input_stride_in_bytes,
					outputPtr, output_w, output_h, output_stride_in_bytes,
					num_channels, alpha_channel, (int)flags, (int)edge_wrap_mode, (int)filter, (int)space);
			}
		}

		public static int stbir_resize_float_generic(float[] input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			float[] output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			int num_channels, int alpha_channel, stbir__resize_flag flags, stbir_edge edge_wrap_mode, stbir_filter filter, stbir_colorspace space)
		{
			fixed (float* inputPtr = input_pixels)
			fixed (float* outputPtr = output_pixels)
			{
				return stbir_resize_float_generic(inputPtr, input_w, input_h, input_stride_in_bytes,
					outputPtr, output_w, output_h, output_stride_in_bytes,
					num_channels, alpha_channel, (int)flags, (int)edge_wrap_mode, (int)filter, (int)space);
			}
		}

		public static int stbir_resize(void* input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			void* output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			stbir_datatype datatype, int num_channels, int alpha_channel, stbir__resize_flag flags,
			stbir_edge edge_mode_horizontal, stbir_edge edge_mode_vertical,
			stbir_filter filter_horizontal, stbir_filter filter_vertical, stbir_colorspace space)
		{
			return stbir_resize(input_pixels, input_w, input_h, input_stride_in_bytes,
				output_pixels, output_w, output_h, output_stride_in_bytes,
				(int)datatype, num_channels, alpha_channel, (int)flags,
				(int)edge_mode_horizontal, (int)edge_mode_vertical, (int)filter_horizontal, (int)filter_vertical, (int)space);
		}

		public static int stbir_resize_subpixel(void* input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			void* output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			stbir_datatype datatype, int num_channels, int alpha_channel, stbir__resize_flag flags,
			stbir_edge edge_mode_horizontal, stbir_edge edge_mode_vertical,
			stbir_filter filter_horizontal, stbir_filter filter_vertical, stbir_colorspace space,
			float x_scale, float y_scale, float x_offset, float y_offset)
		{
			return stbir_resize_subpixel(input_pixels, input_w, input_h, input_stride_in_bytes,
				output_pixels, output_w, output_h, output_stride_in_bytes,
				(int)datatype, num_channels, alpha_channel, (int)flags,
				(int)edge_mode_horizontal, (int)edge_mode_vertical, (int)filter_horizontal, (int)filter_vertical, (int)space,
				x_scale, y_scale, x_offset, y_offset);
		}

		public static int stbir_resize_region(void* input_pixels, int input_w, int input_h, int input_stride_in_bytes,
			void* output_pixels, int output_w, int output_h, int output_stride_in_bytes,
			stbir_datatype datatype, int num_channels, int alpha_channel, stbir__resize_flag flags,
			stbir_edge edge_mode_horizontal, stbir_edge edge_mode_vertical,
			stbir_filter filter_horizontal, stbir_filter filter_vertical, stbir_colorspace space,
			float s0, float t0, float s1, float t1)
		{
			return stbir_resize_region(input_pixels, input_w, input_h, input_stride_in_bytes,
				output_pixels, output_w, output_h, output_stride_in_bytes,
				(int)datatype, num_channels, alpha_channel, (int)flags,
				(int)edge_mode_horizontal, (int)edge_mode_vertical, (int)filter_horizontal, (int)filter_vertical, (int)space,
				s0, t0, s1, t1);
		}
	}
}